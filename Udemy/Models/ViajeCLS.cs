using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Udemy.Models
{
    public class ViajeCLS
    {
        [Display(Name = "Id viaje")]
        public int iidviaje { get; set; }

        //Lugar origen
        [Display(Name = "Lugar origen")]
        [Required]
        public int iidLugarOrigen { get; set; }

        // Lugar destino
        [Display(Name = "Lugar destino")]
        [Required]
        public int iidLugarDestino { get; set; }

        // Lugar Precio
        [Display(Name = "Precio")]
        [Required]
        [Range(0, 100000, ErrorMessage = "Rango fuera de indices")]
        public decimal precio { get; set; }

        [Display(Name = "Fecha Viaje")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fechaViaje { get; set; }
        [Display(Name = "Bus")]
        [Required]
        public int iidBus { get; set; }
        [Display(Name = "Numero Asientos Disponibles")]
        [Required]
        public int numeroAsientosDisponibles { get; set; }

        //Propiedades adicionales
        [Display(Name = "Lugar Origen")]

        public string nombreLugarOrigen { get; set; }
        [Display(Name = "Lugar destino")]

        public string nombreLugarDestino { get; set; }
        [Display(Name = "Nombre Bus")]

        public string nombreBus { get; set; }

        //Propiedades adicionales

        public string nombreFoto { get; set; }

        public string mensaje { get; set; } // mensaje si la foto es nula

        public string fechaViajeCadena { get; set; }

        public string extension { get; set; } // extenión para mostrar la foto

        public string fotoRecuperCadena { get; set; }

    }
}