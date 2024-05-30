using ApiRestEDC.Models;
using Dapper;
using System.Data;

namespace ApiRestEDC.Repositories.Impl
{
    public class EstadoCuentaRepositoryImpl : IEstadoCuentaRepository
    {
        private readonly IDbConnection _dbConnection;

        public EstadoCuentaRepositoryImpl(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> save(EstadoCuenta estadoCuenta)
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

            return id;
        }

        public async Task<IEnumerable<EstadoCuenta>> findAll()
        {
            var estadoCuentas = await _dbConnection.QueryAsync<EstadoCuenta>(
                "SeleccionarTodosEstadoCuenta",
                commandType: CommandType.StoredProcedure);
            return estadoCuentas;
        }


        public async Task<EstadoCuenta> findById(int id)
        {
            var estadoCuenta = await _dbConnection.QuerySingleOrDefaultAsync<EstadoCuenta>(
                "SeleccionarEstadoCuentaPorID",
                new { id = id },
                commandType: CommandType.StoredProcedure);
            return estadoCuenta;
        }
    }

}
