using ApiRestEDC.Models;

namespace ApiRestEDC.Repositories
{
    public interface IEstadoCuentaRepository
    {
        public Task<IEnumerable<EstadoCuenta>> findAll();

        public Task<EstadoCuenta> findById(int id);
        public Task<int> save(EstadoCuenta estadoCuenta);
    }
}
