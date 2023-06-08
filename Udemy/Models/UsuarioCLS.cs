using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Udemy.Models
{
    public class UsuarioCLS
    {
        //Id Usuario
        public int iidusuario { get; set; }

        //Nombre de Usuario
        [Required]    
        public string nombreusuario { get; set; }

        //Contraseña
        [Required]
        public string contra { get; set; }

        //ID uSUARIO
        public string iidtipousuario { get; set; }

        //IID
        [Required]
        public int iid { get; set; }

        //iidRol
        public int iidrol { get; set; }

        //Propiedad Adicional

        public string nombrePersona { get; set; }
        public string nombreRol { get; set; }
        public string nombreTipoEmpleado { get; set; }

        public string nombrePersonaEnviar { get; set; }
    }
}