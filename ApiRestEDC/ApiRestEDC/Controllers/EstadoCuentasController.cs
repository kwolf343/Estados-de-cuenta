using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Dapper;
using ApiRestEDC.Models;

namespace ApiRestEDC.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EstadoCuentasController : ControllerBase
    {
        private readonly IDbConnection _dbConnection;

        public EstadoCuentasController(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        [HttpGet]
        public async Task<IEnumerable<EstadoCuenta>> GetEstadoCuenta()
        {
            var estadoCuentas = await _dbConnection.QueryAsync<EstadoCuenta>(
                "SeleccionarTodosEstadoCuenta",
                commandType: CommandType.StoredProcedure);

            foreach (var estadoCuenta in estadoCuentas)
            {
                var detalles = await _dbConnection.QueryAsync<DetalleEstadoCuenta>(
                    "SeleccionarDetalleEstadoCuentaPorIDEstadoCuenta",
                    new { EstadoCuentaID = estadoCuenta.Id },
                    commandType: CommandType.StoredProcedure);

                estadoCuenta.DetalleEstadoCuenta = detalles.ToList();
            }

            return estadoCuentas;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoCuenta>> GetEstadoCuentaPorID(int id)
        {
            var estadoCuenta = await _dbConnection.QuerySingleOrDefaultAsync<EstadoCuenta>(
                "SeleccionarEstadoCuentaPorID",
                new { id = id },
                commandType: CommandType.StoredProcedure);

            if (estadoCuenta == null)
            {
                return NotFound();
            }

            var detalles = await _dbConnection.QueryAsync<DetalleEstadoCuenta>(
                "SeleccionarDetalleEstadoCuentaPorIDEstadoCuenta",
                new { EstadoCuentaID = estadoCuenta.Id },
                commandType: CommandType.StoredProcedure);

            estadoCuenta.DetalleEstadoCuenta = detalles.ToList();

            return estadoCuenta;
        }

        [HttpGet("{id}/{fecha}")]
        public async Task<ActionResult<EstadoCuenta>> GetEstadoCuentaPorIdYFecha(int id, DateTime fecha)
        {
            var estadoCuenta = await _dbConnection.QuerySingleOrDefaultAsync<EstadoCuenta>(
                "SeleccionarEstadoCuentaPorID",
                new { id = id },
                commandType: CommandType.StoredProcedure);

            if (estadoCuenta == null)
            {
                return NotFound();
            }

            var detalles = await _dbConnection.QueryAsync<DetalleEstadoCuenta>(
                "SeleccionarDetalleEstadoCuentaPorIdYFecha",
                new { IdEstadoCuenta = id, Fecha = fecha },
                commandType: CommandType.StoredProcedure);

            estadoCuenta.DetalleEstadoCuenta = detalles.ToList();

            //Calcular el saldo basado en los detalles dentro del rango de fecha proporcionado
            estadoCuenta.Saldo = detalles.Sum(d => d.Monto);

            return estadoCuenta;
        }


        [HttpPost]
        public async Task<IActionResult> PostEstadoCuenta([FromBody] EstadoCuenta estadoCuenta)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@NumeroTarjeta", estadoCuenta.NumeroTarjeta);
            parameters.Add("@Nombres", estadoCuenta.Nombres);
            parameters.Add("@Apellidos", estadoCuenta.Apellidos);
            parameters.Add("@Cuenta", estadoCuenta.Cuenta);
            parameters.Add("@Limite", estadoCuenta.Limite);
            parameters.Add("@Status", estadoCuenta.Status);
            parameters.Add("@PorcentajeInteresConfigurable", estadoCuenta.PorcentajeInteresConfigurable);
            parameters.Add("@PorcentajeConfigurableSaldoMinimo", estadoCuenta.PorcentajeConfigurableSaldoMinimo);

            var id = await _dbConnection.ExecuteScalarAsync<int>(
                "InsertarEstadoCuenta",
                parameters,
                commandType: CommandType.StoredProcedure);

            estadoCuenta.Id = id;

            return CreatedAtAction(nameof(GetEstadoCuentaPorID), new { id = id }, estadoCuenta);
        }

    }
}
