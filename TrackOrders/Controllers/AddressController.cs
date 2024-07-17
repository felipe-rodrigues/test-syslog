using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TrackOrders.ViewModels;

namespace TrackOrders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {

        public AddressController()
        {
            
        }

        [HttpGet("cep/{cep}")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchCEP(string cep)
        {

            //TODO: mover lógica para um serviço, possivelmente criar um client 
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://brasilapi.com.br/");
            var response = await client.GetAsync($"api/cep/v1/{cep}");

            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<AddressResponse>();
                return Ok(content);
            }
            else
            {
                return BadRequest("CEP não encontrado");
            }
        }
    }
}
