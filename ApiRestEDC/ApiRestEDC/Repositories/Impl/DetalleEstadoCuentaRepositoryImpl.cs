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
        public async Task<IEnumerable<DetalleEstadoCuenta>> GetByEstadoCuentaIdAsync(int estadoCuentaId)
        {
            var detalles = await _dbConnection.QueryAsync<DetalleEstadoCuenta>(
                "SeleccionarDetalleEstadoCuentaPorIDEstadoCuenta",
                new { EstadoCuentaID = estadoCuentaId },
                commandType: CommandType.StoredProcedure);
            return detalles;
        }

        public async Task<IEnumerable<DetalleEstadoCuenta>> GetByIdAndDateAsync(int estadoCuentaId, DateTime fecha)
        {
            var detalles = await _dbConnection.QueryAsync<DetalleEstadoCuenta>(
                "SeleccionarDetalleEstadoCuentaPorIdYFecha",
                new { IdEstadoCuenta = estadoCuentaId, Fecha = fecha },
                commandType: CommandType.StoredProcedure);
            return detalles;
        }
    }
}
