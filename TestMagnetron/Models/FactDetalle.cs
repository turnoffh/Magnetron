using System;
using System.Collections.Generic;

namespace TestMagnetron.Models
{
    public partial class FactDetalle
    {
        public int FdetId { get; set; }
        public string FdetLinea { get; set; } = null!;
        public int FdetCantidad { get; set; }
        public int FdetProdId { get; set; }
        public int FdetFencId { get; set; }

        public virtual FactEncabezado FdetFenc { get; set; } = null!;
        public virtual Producto FdetProd { get; set; } = null!;
    }
}
