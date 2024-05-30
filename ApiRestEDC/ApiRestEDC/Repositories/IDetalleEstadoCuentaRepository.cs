using ApiRestEDC.Models;

namespace ApiRestEDC.Repositories
{
    public interface IDetalleEstadoCuentaRepository
    {
        public Task<IEnumerable<DetalleEstadoCuenta>> GetByEstadoCuentaIdAsync(int estadoCuentaId);
        public Task<IEnumerable<DetalleEstadoCuenta>> GetByIdAndDateAsync(int estadoCuentaId, DateTime fecha);
    }
}
