using ApiRestEDC.Models;
using Dapper;
using System.Data;

namespace ApiRestEDC.Repositories.Impl
{
    public class DetalleEstadoCuentaRepositoryImpl : IDetalleEstadoCuentaRepository
    {
        private readonly IDbConnection _dbConnection;

        public DetalleEstadoCuentaRepositoryImpl(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<IEnumerable<DetalleEstadoCuenta>> findById(int estadoCuentaId)
        {
            var detalles = await _dbConnection.QueryAsync<DetalleEstadoCuenta>(
                "SeleccionarDetalleEstadoCuentaPorIDEstadoCuenta",
                new { EstadoCuentaID = estadoCuentaId },
                commandType: CommandType.StoredProcedure);
            return detalles;
        }

        public async Task<int> Save(DetalleEstadoCuenta detalleEstadoCuenta)
        {
            var parameters = new
            {
                idEstadoCuenta = detalleEstadoCuenta.IdEstadoCuenta,
                Descripcion = detalleEstadoCuenta.Descripcion,
                Fecha = detalleEstadoCuenta.Fecha,
                Monto = detalleEstadoCuenta.Monto,
                Accion = detalleEstadoCuenta.Accion,
                NumAutorizacion = detalleEstadoCuenta.NumAutorizacion
            };

            var id = await _dbConnection.ExecuteScalarAsync<int>(
                "InsertarDetalleEstadoCuenta",
                parameters,
                commandType: CommandType.StoredProcedure);

            return id;
        }
    }
}
