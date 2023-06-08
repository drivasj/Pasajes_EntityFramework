using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Udemy.Models
{
    public class MarcaCLS
    {
        [Display(Name ="ID Marca")]
        public int iidmarca { get; set; }

        [Display(Name = "Nombre Marca")]//Nombre a mostrar en la vista
        [Required] //Campo requerido
        [StringLength(100,ErrorMessage ="La longitud Maxima es 100")]//Longitud del campo
        public string nombre { get; set; }

        [Display(Name = "Descripción Marca")]
        [Required]
        [StringLength(200,ErrorMessage ="La longitud maxima es 200")]
        public string descripcion { get; set; }

        public int bhabilitadp { get; set; }

        //Añado una propiedad (Errores de validación)

        public string mensajeError { get; set; }


    }
}