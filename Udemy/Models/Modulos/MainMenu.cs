
namespace Udemy.Models.Modulos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("MainMenu")]
    public class MainMenu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MainMenu()
        {
            SubMenus = new HashSet<SubMenu2>();
        }

        public int id { get; set; }

        public string menuname { get; set; }

        public string menuurl { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubMenu2> SubMenus { get; set; }
    }
}