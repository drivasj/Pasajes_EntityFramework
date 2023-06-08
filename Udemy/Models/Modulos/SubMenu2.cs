using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Udemy.Models.Modulos
{
    [Table("SubMenu")]
    public class SubMenu2
    {
        public int id { get; set; }

        public string submenuname { get; set; }

        public string submenuurl { get; set; }

        public int? mainmenuid { get; set; }

        public virtual MainMenu MainMenu { get; set; }
    }
}