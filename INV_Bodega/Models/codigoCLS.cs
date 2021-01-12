using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INV_Bodega.Models
{
 

    public class codigoCLS
    {
        public string Barras { get; set; }
        public string Referencia { get; set; }
        public string Talla { get; set; }
        public string Color { get; set; }
        public int Rowid_canasta { get; set; }
        public int Cant { get; set; }
    }
    public class BodegaCLS
    {
        public int Pasillo { get; set; }
        public string Bloque { get; set; }
        public int Canasta { get; set; }
        public string Usuario{ get; set; } 
        public string Bodega{ get; set; } 
        public string Id{ get; set; }

        public string Barras { get; set; }
        public string Referencia { get; set; }
        public string Talla { get; set; }
        public string Color { get; set; }
        public int Rowid_canasta { get; set; }
        public int Cant { get; set; }
    }
}
