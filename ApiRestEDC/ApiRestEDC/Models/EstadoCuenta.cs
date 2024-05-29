using System;
using System.Collections.Generic;

namespace ApiRestEDC.Models;

public class EstadoCuenta
{
    public int Id { get; set; }
    public string NumeroTarjeta { get; set; } = null!;
    public string Nombres { get; set; } = null!;
    public string Apellidos { get; set; } = null!;
    public int Cuenta { get; set; }
    public int Status { get; set; }
    public decimal Saldo { get; set; }
    public decimal Limite { get; set; }
    public decimal PorcentajeInteresConfigurable { get; set; }
    public decimal PorcentajeConfigurableSaldoMinimo { get; set; }
    public ICollection<DetalleEstadoCuenta> DetalleEstadoCuenta { get; set; } = new List<DetalleEstadoCuenta>();
}

