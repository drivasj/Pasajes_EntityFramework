using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Udemy.Models
{
    public class TipoUsuarioCLS
    {
        //ID  ID Tipo Usuario
        [Display(Name = "ID Tipo Usuario")]
        public int iidtipousuario { get; set; }

        //Nombre
        [Display(Name = "Nombre")]
        [Required]
        [StringLength(150,ErrorMessage ="Longitud Maxima 150")]
        public string nombre { get; set; }

        //Descripción
        [Display(Name = "Descripción")]
        [Required]
        [StringLength(250, ErrorMessage = "Longitud Maxima 250")]
        public string descripcion { get; set; }

        //Habilitado ?
        public int bhabilitado { get; set; }
    }
}