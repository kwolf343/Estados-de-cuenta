using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiRestEDC.Models;
using ApiRestEDC.Repositories;

namespace ApiRestEDC.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EstadoCuentasController : ControllerBase
    {
        private readonly IEstadoCuentaRepository _estadoCuentaRepository;
        private readonly IDetalleEstadoCuentaRepository _detalleEstadoCuentaRepository;

        public EstadoCuentasController(
            IEstadoCuentaRepository estadoCuentaRepository,
            IDetalleEstadoCuentaRepository detalleEstadoCuentaRepository)
        {
            _estadoCuentaRepository = estadoCuentaRepository;
            _detalleEstadoCuentaRepository = detalleEstadoCuentaRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<EstadoCuenta>> GetEstadoCuenta()
        {
            var estadoCuentas = await _estadoCuentaRepository.findAll();
            foreach (var estadoCuenta in estadoCuentas)
            {
                var detalles = await _detalleEstadoCuentaRepository.GetByEstadoCuentaIdAsync(estadoCuenta.Id);
                estadoCuenta.DetalleEstadoCuenta = detalles.ToList();
            }

            return estadoCuentas;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoCuenta>> GetEstadoCuentaPorID(int id)
        {
            var estadoCuenta = await _estadoCuentaRepository.findById(id);
            if (estadoCuenta == null)
            {
                return NotFound();
            }

            var detalles = await _detalleEstadoCuentaRepository.GetByEstadoCuentaIdAsync(estadoCuenta.Id);
            estadoCuenta.DetalleEstadoCuenta = detalles.ToList();

            return estadoCuenta;
        }

        [HttpPost]
        public async Task<IActionResult> PostEstadoCuenta([FromBody] EstadoCuenta estadoCuenta)
        {
            var id = await _estadoCuentaRepository.save(estadoCuenta);
            estadoCuenta.Id = id;

            return CreatedAtAction(nameof(GetEstadoCuentaPorID), new { id = id }, estadoCuenta);
        }
    }
}
