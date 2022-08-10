using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokeAcademy.API.Models;

namespace PokeAcademy.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonsController : Controller
    {
        //URL principal de acesso a API do pokeapi
        private const string ALL_POKEMON_URL = "http://pokeapi.co/api/v2/pokemon";

        //URL externa de outra api localhost para a validação do cabeçalho do header
        private const string EXTERNAL_BASE_URL = "http://localhost:7235";

        //Falso Barear TOKEN desafio para implementar posteriormente

        private const string TOKEN = "Bearer 32313213dqdadadsadqwdadsadasdsadsadsadqewqe13231";

        public PokemonsController()
        {
        }

        //Usando o HttpClient padrão para consumo de api
        [HttpGet("/Http")]

        public async Task<IActionResult> GetAllPokemonsHTTP(int limit )
        {
            if (limit < 1) return BadRequest("Limit must be greater than zero.");
            else
            {
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(ALL_POKEMON_URL+"/"+limit );
                var data = await response.Content.ReadAsStringAsync();
                return Ok(data);
            }
            }

        // Framework Flurl 
        [HttpGet("/Flurl")]
        public async Task<IActionResult> GetAllPokemonsFLURL(int limit)
        {
            //FLurl utiliza métodos de extensão em cima de "spring" 
            var result = await ALL_POKEMON_URL
            //SetQueryParams = Parametros impostos pela api 
            .SetQueryParams(new { limit = limit })    /* Exemplo de parametro de api ->  ?limit =151 */
            //GetJsonAsync captura o retorno da api enquanto a classe NamedAPIResourceList faz a deserialização dos dados
            .GetJsonAsync<NamedAPIResourceList>();

            var viewModelList = new PokemonListViewModel
            {
                Count = result.Count, //Criação de um contador para enumeração dos dados de returno
                Pokemons = result.Results.Select(result =>
                {
                    var lastSegment = new Uri(result.Url).Segments.Last();
                    var id = lastSegment.Remove(lastSegment.Length - 1);// Ao fazer a captura a função lastSegment.Length - 1 faz a remoção de 1 caracter do retorno começando de tras para a frente

                    return new PokemonListItemViewModel { Name = result.Name, Id = int.Parse(id)};
                })
            };
            return Ok(viewModelList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIdPokemons(int id)
        {
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> PostPokemons(Ping ping)
        {
            return Ok();
        }
    }

    public class Ping
    {

    }
}