using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TestMagnetron.Models
{
    public partial class FactDetalle
    {
        public int FdetId { get; set; }
        public string FdetLinea { get; set; } = null!;
        public int FdetCantidad { get; set; }
        public int FdetProdId { get; set; }
        public int FdetFencId { get; set; }

        [JsonIgnore]
        public virtual FactEncabezado FdetFenc { get; set; } = null!;
        public virtual Producto FdetProd { get; set; } = null!;
    }

    public class FactDetalleInputVM
    {
        public string Linea { get; set; } = null!;
        public int Cantidad { get; set; }
        public int ProductoId { get; set; }
    }

    public class FactDetalleUpdateVM
    {
        public string Linea { get; set; } = null!;
        public int Cantidad { get; set; }
        public int ProductoId { get; set; }
    }
}
