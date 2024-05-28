using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiEDC.Data;
using ApiEDC.Models;

namespace ApiEDC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleEstadoCuentasController : ControllerBase
    {
        private readonly MyAppContext _context;

        public DetalleEstadoCuentasController(MyAppContext context)
        {
            _context = context;
        }

        // GET: api/DetalleEstadoCuentas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleEstadoCuenta>>> GetDetalleEstadoCuenta()
        {
            return await _context.DetalleEstadoCuenta.ToListAsync();
        }

        // GET: api/DetalleEstadoCuentas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetalleEstadoCuenta>> GetDetalleEstadoCuenta(int id)
        {
            var detalleEstadoCuenta = await _context.DetalleEstadoCuenta.FindAsync(id);

            if (detalleEstadoCuenta == null)
            {
                return NotFound();
            }

            return detalleEstadoCuenta;
        }

        // PUT: api/DetalleEstadoCuentas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetalleEstadoCuenta(int id, DetalleEstadoCuenta detalleEstadoCuenta)
        {
            if (id != detalleEstadoCuenta.Id)
            {
                return BadRequest();
            }

            _context.Entry(detalleEstadoCuenta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetalleEstadoCuentaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DetalleEstadoCuentas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DetalleEstadoCuenta>> PostDetalleEstadoCuenta(DetalleEstadoCuenta detalleEstadoCuenta)
        {
            _context.DetalleEstadoCuenta.Add(detalleEstadoCuenta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetalleEstadoCuenta", new { id = detalleEstadoCuenta.Id }, detalleEstadoCuenta);
        }

        // DELETE: api/DetalleEstadoCuentas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalleEstadoCuenta(int id)
        {
            var detalleEstadoCuenta = await _context.DetalleEstadoCuenta.FindAsync(id);
            if (detalleEstadoCuenta == null)
            {
                return NotFound();
            }

            _context.DetalleEstadoCuenta.Remove(detalleEstadoCuenta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DetalleEstadoCuentaExists(int id)
        {
            return _context.DetalleEstadoCuenta.Any(e => e.Id == id);
        }
    }
}
