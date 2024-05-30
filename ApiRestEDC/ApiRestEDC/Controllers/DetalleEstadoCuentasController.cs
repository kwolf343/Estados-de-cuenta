using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiRestEDC.Datos;
using ApiRestEDC.Models;
using ApiRestEDC.Repositories;

namespace ApiRestEDC.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DetalleEstadoCuentasController : ControllerBase
    {
        private readonly IDetalleEstadoCuentaRepository _detalleEstadoCuentaRepository;

        public DetalleEstadoCuentasController(IDetalleEstadoCuentaRepository detalleEstadoCuentaRepository)
        {
            _detalleEstadoCuentaRepository = detalleEstadoCuentaRepository;
        }

        // GET: DetalleEstadoCuentas/5
        [HttpGet("{estadoCuentaId}")]
        public async Task<IEnumerable<DetalleEstadoCuenta>> GetDetalleEstadoCuenta(int estadoCuentaId)
        {
            var detalles = await _detalleEstadoCuentaRepository.findById(estadoCuentaId);
            return detalles;
        }

        // POST: DetalleEstadoCuentas
        [HttpPost]
        public async Task<IActionResult> PostDetalleEstadoCuenta([FromBody] DetalleEstadoCuenta detalleEstadoCuenta)
        {
            var id = await _detalleEstadoCuentaRepository.Save(detalleEstadoCuenta);
            detalleEstadoCuenta.Id = id;

            return CreatedAtAction(nameof(GetDetalleEstadoCuenta), new { estadoCuentaId = detalleEstadoCuenta.IdEstadoCuenta }, detalleEstadoCuenta);
        }
    }
}
