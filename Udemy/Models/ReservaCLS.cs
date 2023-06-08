using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Udemy.Models
{
    public class ReservaCLS
    {
        //ID del viaje
        public int iidViaje { get; set; }

        public string nombreArchivo { get; set; }
        public byte[] foto { get; set; }
        public string lugarOrigen { get; set; }
        public string lugarDestino { get; set; }
        public decimal precio { get; set; }
        public DateTime FechaViaje { get; set; }
        public string nombreBus { get; set; }
        public string descripcionBus { get; set; }
        public int asientosDisponibles { get; set; }
        public int totalAsientos { get; set; }

        public int iidBus { get; set; }

        public int cantidad { get; set; }

    }
}