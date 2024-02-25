using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TestMagnetron.Models
{
    public partial class FactEncabezado
    {
        public FactEncabezado()
        {
            FactDetalles = new HashSet<FactDetalle>();
        }

        public int FencId { get; set; }
        public string FencNumero { get; set; } = null!;
        public DateTime FencFecha { get; set; }
        public int FencPerId { get; set; }
        public virtual Persona FencPer { get; set; } = null!;
        public virtual ICollection<FactDetalle> FactDetalles { get; set; }
    }

    public class FactEncabezadoInputVM
    {
        public string Numero { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public int PersonaId { get; set; }
        public virtual ICollection<FactDetalleInputVM> Detalles { get; set; }
    }

    public class FactEncabezadoUpdateVM
    {
        public string Numero { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public int PersonaId { get; set; }
    }
}
