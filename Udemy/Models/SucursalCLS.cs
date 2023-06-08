using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Udemy.Models
{
    public class SucursalCLS
    {  
        // ID
        [Display (Name="ID Sucursal")]
        public int iidsucursal { get; set; }

        //Nombre
        [Display(Name ="Nombre Sucursal")]
        [Required]
        [StringLength(100, ErrorMessage = "La longitud Maxima es 100")]
        public string nombre { get; set; }

        //Dirección
        [Display(Name ="Dirección")]
        [Required]
        [StringLength(200, ErrorMessage = "La longitud Maxima es 100")]
        public string direccion { get; set; }

        //Telefono
        [Display(Name ="Telefono Sucursal")]
        [Required]
        [StringLength(10, ErrorMessage = "La longitud Maxima es 100")]
        public string telefono { get; set; }

        //Email
        [Display(Name ="Email Sucursal")]
        [Required]
        [StringLength(100, ErrorMessage = "La longitud Maxima es 100")]
        [EmailAddress(ErrorMessage ="Ingrese un email valido")]
        public string email { get; set; }

        //Fecha
        [Display(Name = "Fecha Apertura")]
   //   [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime fechaApertura { get; set; }

        //Habilitado
        public int bhabilitado { get; set; }

        // (Errores de validación)
        public string mensajeError { get; set; }

    }
}