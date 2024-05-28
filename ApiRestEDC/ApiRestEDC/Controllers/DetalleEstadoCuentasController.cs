using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiRestEDC.Datos;
using ApiRestEDC.Models;
using System.Data;
using Dapper;

namespace ApiRestEDC.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DetalleEstadoCuentasController : ControllerBase
    {
        private readonly IDbConnection _dbConnection;

        public DetalleEstadoCuentasController(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<DetalleEstadoCuenta>> GetDetalleEstadoCuentaPorID(int id)
        {
            var detalles = await _dbConnection.QueryAsync<DetalleEstadoCuenta>(
                "SeleccionarDetalleEstadoCuentaPorIDEstadoCuenta",
                new { EstadoCuentaID = id },
                commandType: CommandType.StoredProcedure);

            return detalles;
        }

        [HttpPost]
        public async Task<IActionResult> PostDetalleEstadoCuenta([FromBody] DetalleEstadoCuenta detalleEstadoCuenta)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idEstadoCuenta", detalleEstadoCuenta.IdEstadoCuenta);
            parameters.Add("@Descripcion", detalleEstadoCuenta.Descripcion);
            parameters.Add("@Fecha", detalleEstadoCuenta.Fecha);
            parameters.Add("@Monto", detalleEstadoCuenta.Monto);
            parameters.Add("@Accion", detalleEstadoCuenta.Accion);
            parameters.Add("@NumAutorizacion", detalleEstadoCuenta.NumAutorizacion);


            var id = await _dbConnection.ExecuteScalarAsync<int>(
                "InsertarDetalleEstadoCuenta",
                parameters,
                commandType: CommandType.StoredProcedure);

            detalleEstadoCuenta.Id = id;

            return CreatedAtAction(nameof(GetDetalleEstadoCuentaPorID), new { id = id }, detalleEstadoCuenta);
        }
    }
}
