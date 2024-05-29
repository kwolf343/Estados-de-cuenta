using System.ComponentModel.DataAnnotations;

namespace FrontEndEDC.Models
{
    public class DetalleEstadoCuenta
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID del estado de cuenta es obligatorio")]
        public int IdEstadoCuenta { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [MaxLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
        public string Descripcion { get; set; } = null!;

        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El monto debe ser un valor positivo")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "La acción es obligatoria")]
        [Range(1, 2, ErrorMessage = "La acción debe ser 1 (compra) o 2 (pago)")]
        public int Accion { get; set; }

        [Required(ErrorMessage = "El número de autorización es obligatorio")]
        public int NumAutorizacion { get; set; }
    }
}
