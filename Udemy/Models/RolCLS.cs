using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Udemy.Models
{
    public class RolCLS
    {
        // ID Rol
        [Display(Name ="ID Rol")]
        public int iidRol { get; set; }

        // Nombre Rol
        [Required]
        [Display(Name = "Nombre Rol")]
        public string nombre { get; set; }

        // Descripción Rol
        [Required]
        [Display(Name = "Descripción Rol")]
        public string descripcion { get; set; }

        // Habilitado?
        public int bhabilitado { get; set; }

        //NRCAJAS ->> Prueba 'nada que ver'
        public string nrcajas { get; set; }
    }
}