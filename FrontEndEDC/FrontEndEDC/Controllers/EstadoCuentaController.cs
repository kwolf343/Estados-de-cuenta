using FrontEndEDC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

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
            try
            {
                var response = await _httpClient.GetAsync("EstadoCuentas");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
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
                return View("Detalle", null);
            }

            return View("Detalle", null);
        }

        [HttpGet]
        public async Task<IActionResult> Compra(int? id)
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
                        estadoCuenta.FechaCompra = DateTime.Today;
                        return View("Compra", estadoCuenta);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                return View("Compra", null);
            }

            return View("Compra", null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Compra(EstadoCuenta estadoCuenta)
        {
            if (ModelState.IsValid)
            {
                if (estadoCuenta.MontoCompra > estadoCuenta.CuotaMinimaAPagar)
                {
                    ModelState.AddModelError("MontoCompra", $"La compra excede el saldo disponible, saldo disponible: ${estadoCuenta.CuotaMinimaAPagar:F2}");
                    return View(estadoCuenta);
                }


                var detalleEDC = new DetalleEstadoCuenta
                {
                    IdEstadoCuenta = estadoCuenta.Id,
                    Descripcion = estadoCuenta.DescripcionCompra,
                    Fecha = estadoCuenta.FechaCompra,
                    Monto = estadoCuenta.MontoCompra,
                    Accion = 1,
                    NumAutorizacion = 123456789
                };

                var json = JsonConvert.SerializeObject(detalleEDC);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync("DetalleEstadoCuentas", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "EstadoCuenta");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ocurrió un error al guardar el detalle del estado de cuenta.");
                }
            }
            return View("Compra", estadoCuenta);
        }
        [HttpGet]
        public async Task<IActionResult> Pago(int? id)
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
                        estadoCuenta.FechaCompra = DateTime.Today;
                        return View("Pago", estadoCuenta);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                return View("Pago", null);
            }

            return View("Pago", null);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pago(EstadoCuenta estadoCuenta)
        {
            ModelState.Remove("DescripcionCompra");

            if (ModelState.IsValid)
            {
                // Verifica si el monto de la compra es menor que la cuota mínima a pagar
                if (estadoCuenta.MontoCompra <= estadoCuenta.CuotaMinimaAPagar)
                {
                    ModelState.AddModelError("MontoCompra", $"La cuota mínima a pagar es: ${estadoCuenta.CuotaMinimaAPagar:F2}");
                    return View(estadoCuenta);
                }
                // Verifica si el monto de la compra es mayor al saldo
                if (estadoCuenta.MontoCompra > estadoCuenta.Saldo)
                {
                    ModelState.AddModelError("MontoCompra", $"No se puede abonar mas de la deuda, deuda: ${estadoCuenta.Saldo:F2}");
                    return View(estadoCuenta);
                }
                var detalleEDC = new DetalleEstadoCuenta
                {
                    IdEstadoCuenta = estadoCuenta.Id,
                    Descripcion = "Pago de TC",
                    Fecha = estadoCuenta.FechaCompra,
                    Monto = estadoCuenta.MontoCompra,
                    Accion = 2,
                    NumAutorizacion = 123456789
                };

                var json = JsonConvert.SerializeObject(detalleEDC);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("DetalleEstadoCuentas", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "EstadoCuenta");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ocurrió un error al guardar el detalle del estado de cuenta.");
                }
            }

            return View("Pago", estadoCuenta);
        }

        [HttpGet]
        public async Task<IActionResult> Transacciones(int? id)
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
                        return View("Transacciones", estadoCuenta);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                return View("Transacciones", null);
            }

            return View("Transacciones", null);
        }
    }
}
