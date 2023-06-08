namespace Udemy.Models.Modulos
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations.Schema;
    public class BDPasajeEntities : DbContext
    {
        public BDPasajeEntities()
           : base("name=BDPasajeEntities")
        {

        }

        public virtual DbSet<MainMenu> MainMenus { get; set; }
        public virtual DbSet<SubMenu2> SubMenus { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MainMenu>()
                .Property(e => e.menuname)
                .IsUnicode(false);
        }
    }
}