using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Udemy.Models
{
    public class ClienteCLS
    {
        // ID
        [Display(Name ="ID Cliente")]
        public int iidcliente { get; set; }

        //Nombre
        [Display(Name ="Nombre ")]
        [Required]
        [StringLength(100, ErrorMessage = "La longitud Maxima es 100")]
        public string nombre { get; set; }

        //Apellido Paterno
        [Display(Name ="Apellido Paterno")]
        [Required]
        [StringLength(150, ErrorMessage = "La longitud Maxima es 100")]
        public string appaterno { get; set; }

        //Apellido Matermo
        [Display(Name = "Apellido Materno")]
        [Required]
        [StringLength(150, ErrorMessage = "La longitud Maxima es 100")]
        public string apmaterno { get; set; }

        // Email
        [Display(Name ="Email")]
        [Required]
        [StringLength(200, ErrorMessage = "La longitud Maxima es 100")]
        [EmailAddress(ErrorMessage = "Ingrese un email valido")]
        public string email { get; set; }

        //Dirección
        [Display(Name ="Dirección")]
        [DataType(DataType.MultilineText)]
        [Required]
        [StringLength(200, ErrorMessage = "La longitud Maxima es 100")]
        public string direccion { get; set; }

        //Telefono Celular
        [Display(Name = "Telefono Celular")]
        [Required]
        [StringLength(10, ErrorMessage = "La longitud Maxima es 100")]
        public string telefonocelular { get; set; }

        //Telefono Fijo
        [Display(Name = "Telefono Fijo")]
        [Required]
        [StringLength(10, ErrorMessage = "La longitud Maxima es 100")]
        public string telefonofijo { get; set; }

        //iidSexo
        [Display(Name = "Sexo")]
        [Required]
        public int iidsexo { get; set; }

        //Añado una propiedad (Errores de validación)

        public string mensajeError { get; set; }

    }
}