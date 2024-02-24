using System;
using System.Collections.Generic;

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
}
