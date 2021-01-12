using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace INV_Bodega.Models
{
    public class UsuarioCLS
    {
        #region UsuarioCLS

        [Display(Name = "id")]
        public int Id { get; set; }
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
        [Display(Name = "Contraseña")]
        public string Contraseña { get; set; }
        [Display(Name = "Correo")]
        public string Correo { get; set; }


        #endregion
    }
}