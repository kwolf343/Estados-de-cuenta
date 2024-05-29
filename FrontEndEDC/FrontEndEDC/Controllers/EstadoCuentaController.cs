using FrontEndEDC.Models;
using FrontEndEDC.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FrontEndEDC.Controllers
{
    public class EstadoCuentaController : Controller
    {
        private readonly HttpClient _httpClient;
        public EstadoCuentaController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7154");
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try {
                var response = await _httpClient.GetAsync("EstadoCuentas");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    //var estadosCuenta = JsonSerializer.Deserialize<EstadoCuenta>(content);
                    var estadosCuenta = JsonConvert.DeserializeObject<IEnumerable<EstadoCuenta>>(content);
                    return View("Index", estadosCuenta);
                }
            }
            catch (Exception ex)
            {
                return View(new List<EstadoCuenta>());
            }
            return View(new List<EstadoCuenta>());
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            try
            {
                var response = await _httpClient.GetAsync($"EstadoCuentas/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var estadoCuenta = JsonConvert.DeserializeObject<EstadoCuenta>(content);
                    if (estadoCuenta != null)
                    {
                        return View("Detalle", estadoCuenta);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
                return StatusCode(500, "Internal server error");
            }

            return NotFound();
        }
    }
}
