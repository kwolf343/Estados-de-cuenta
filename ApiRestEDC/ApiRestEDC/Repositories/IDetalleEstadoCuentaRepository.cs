using ApiRestEDC.Models;

namespace ApiRestEDC.Repositories
{
    public interface IDetalleEstadoCuentaRepository
    {
        public Task<IEnumerable<DetalleEstadoCuenta>> findById(int estadoCuentaId);
        public Task<int> Save(DetalleEstadoCuenta detalleEstadoCuenta);
    }
}
