//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Udemy.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pagina
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pagina()
        {
            this.RolPagina = new HashSet<RolPagina>();
        }
    
        public int IIDPAGINA { get; set; }
        public string MENSAJE { get; set; }
        public string ACCION { get; set; }
        public string CONTROLADOR { get; set; }
        public Nullable<int> BHABILITADO { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RolPagina> RolPagina { get; set; }
    }
}
