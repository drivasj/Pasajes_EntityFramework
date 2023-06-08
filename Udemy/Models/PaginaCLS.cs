using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Udemy.Models
{
    public class PaginaCLS
    {
        // ID Pagina
        [Display(Name = "ID Pagina")]
        public int iidpagina { get; set; }
        [Required]
        // Mensaje
        [Display(Name = "Tiulo de link")]
        public string mensaje { get; set; }
        [Required]

        // Acción
        [Display(Name = " Nombre de la Acción")]
        public string accion { get; set; }
        [Required]

        // Controlador
        [Display(Name = "Controlador")]
        public string controlador { get; set; }
       

        // ID Pagina
        public string bhabilitdo { get; set; }

        // propiedada adicional 

        public string mensajeFiltro { get; set; }
        

    }
}