using System;
using System.Collections.Generic;

namespace TestMagnetron.Models
{
    public partial class VistaTotalFacturado
    {
        public int PerId { get; set; }
        public string? Descripcion { get; set; }
        public string PerDocumento { get; set; } = null!;
        public string? Cliente { get; set; }
        public decimal TotalFacturado { get; set; }
    }
}
