namespace ApiRestEDC.Models
{
    public class DetalleEstadoCuenta
    {
        public int Id { get; set; }
        public int IdEstadoCuenta { get; set; }
        public string Descripcion { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public int Accion { get; set; }
        public int NumAutorizacion { get; set; }
    }
}
