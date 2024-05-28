using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiEDC.Data;
using ApiEDC.Models;
using Dapper;
using System.Data;

namespace ApiEDC.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<EstadoCuenta> GetEstadoCuentaPorID(int id)
        {
            var estadoCuenta = await _dbConnection.QuerySingleOrDefaultAsync<EstadoCuenta>(
                "SeleccionarEstadoCuentaPorID",
                new { EstadoCuentaID = id },
                commandType: CommandType.StoredProcedure);

            if (estadoCuenta != null)
            {
                var detalles = await _dbConnection.QueryAsync<DetalleEstadoCuenta>(
                    "SeleccionarDetalleEstadoCuentaPorIDEstadoCuenta",
                    new { EstadoCuentaID = estadoCuenta.Id },
                    commandType: CommandType.StoredProcedure);

                estadoCuenta.DetalleEstadoCuenta = detalles.ToList();
            }

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
            parameters.Add("@Limite", estadoCuenta.Cuenta);
            parameters.Add("@Status", estadoCuenta.Status);

            var id = await _dbConnection.ExecuteScalarAsync<int>(
                "InsertarEstadoCuenta",
                parameters,
                commandType: CommandType.StoredProcedure);

            // Set the Id of the inserted estadoCuenta
            estadoCuenta.Id = id;

            return CreatedAtAction(nameof(GetEstadoCuentaPorID), new { id = id }, estadoCuenta);
        }
    }
}
