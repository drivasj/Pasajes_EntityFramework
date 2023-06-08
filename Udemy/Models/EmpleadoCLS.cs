using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Udemy.Models
{
    public class EmpleadoCLS
    {
        //ID Empleado
        [Display (Name ="Id Empleado")]
        public int iidEmpleado { get; set; }

        //Nombre
        [Display(Name = "Nombre")]
        [Required]
        [StringLength(100,ErrorMessage ="Longitud maxima 100")]
        public string nombre { get; set; }

        // Apellido Paterno
        [Display(Name = "Apellido Paterno")]
        [Required]
        [StringLength(200, ErrorMessage = "Longitud maxima 200")]
        public string apPaterno { get; set; }

        //Apellido Materno
        [Display(Name = "Apellido Materno")]
        [Required]
        [StringLength(200, ErrorMessage = "Longitud maxima 200")]
        public string apMaterno { get; set; }

        //Fecha Contrato
        [Display(Name = "Fecha de Contrato")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fechaContrato { get; set; }

        //ID Tipo Usuario
        [Display(Name = "Tipo Usuario")]
        [Required]
        public int iidtipoUsuario { get; set; }

        //ID Tipo Contrato
        [Display(Name = "Tipo Contrato")]
        [Required]
        public int iidtipoContrato { get; set; }

        //ID Sexo
        [Display(Name = "Sexo")]
        [Required]
        public int iidSexo { get; set; }

        //Sueldo
        [Display(Name = "Sueldo")]
        [Required]
        [Range(0,100000,ErrorMessage ="Fuera de rango")]
        public decimal sueldo { get; set; }

        //Habilitado
        [Display(Name = "Habilitado")]
        public int bhabilitado { get; set; }

        // Propiedades adicionales
        [Display(Name = "Tipo Contrato")]
        public string nombreTipoContrato { get; set; }
        [Display(Name = "Tipo Usuario")]
        public string nombreTipoUsuario { get; set; }

        //Añado una propiedad (Errores de validación)
        public string mensajeError { get; set; }

    }
}