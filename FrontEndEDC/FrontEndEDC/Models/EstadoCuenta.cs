using System.ComponentModel.DataAnnotations;

namespace FrontEndEDC.Models
{
    public class EstadoCuenta
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de tarjeta es obligatorio")]
        [MaxLength(20, ErrorMessage = "El número de tarjeta no puede exceder 20 caracteres")]
        public string NumeroTarjeta { get; set; } = null!;

        [Required(ErrorMessage = "Los nombres son obligatorios")]
        [MaxLength(100, ErrorMessage = "Los nombres no pueden exceder 100 caracteres")]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [MaxLength(100, ErrorMessage = "Los apellidos no pueden exceder 100 caracteres")]
        public string Apellidos { get; set; } = null!;

        [Required(ErrorMessage = "El número de cuenta es obligatorio")]
        public int Cuenta { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [Range(0, 1, ErrorMessage = "El estado debe ser 0 o 1")]
        public int Status { get; set; }

        public decimal Saldo { get; set; }

        [Required(ErrorMessage = "El límite es obligatorio")]
        public decimal Limite { get; set; }
        [Required(ErrorMessage = "El porcentaje interes configurable es obligatorio")]
        public decimal PorcentajeInteresConfigurable { get; set; }
        [Required(ErrorMessage = "El porcentaje configurable saldo minimo es obligatorio")]
        public decimal PorcentajeConfigurableSaldoMinimo { get; set; }
        public List<DetalleEstadoCuenta> DetalleEstadoCuenta { get; set; } = new List<DetalleEstadoCuenta>();
    }
}
