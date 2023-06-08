using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Udemy.Models;

namespace Udemy.Controllers
{
    public class RolController : Controller
    {
        // GET: Rol Index
        public ActionResult Index()
        {
            List<RolCLS> listaRol = new List<RolCLS>();
            using (var bd = new BDPasajeEntities())
            {
                listaRol = (from rol in bd.Rol
                            where rol.BHABILITADO == 1
                            select new RolCLS
                            {
                                iidRol = rol.IIDROL,
                                nombre = rol.NOMBRE,
                                descripcion = rol.DESCRIPCION,
                                nrcajas = rol.nrcajas
                            }).ToList();
            }
            return View(listaRol);
        }

        //Filtro Rol
        public ActionResult Filtro(string nombreRol)
        {

            List<RolCLS> listaRol = new List<RolCLS>();
            using (var bd = new BDPasajeEntities())
            {
                if (nombreRol == null)
                    listaRol = (from rol in bd.Rol
                                where rol.BHABILITADO == 1
                                select new RolCLS
                                {
                                    iidRol = rol.IIDROL,
                                    nombre = rol.NOMBRE,
                                    descripcion = rol.DESCRIPCION,
                                    nrcajas = rol.nrcajas
                                }).ToList();
                else
                    listaRol = (from rol in bd.Rol
                                where rol.BHABILITADO == 1
                               && rol.NOMBRE.Contains(nombreRol)
                                select new RolCLS
                                {
                                    iidRol = rol.IIDROL,
                                    nombre = rol.NOMBRE,
                                    descripcion = rol.DESCRIPCION,
                                    nrcajas = rol.nrcajas
                                }).ToList();
            }
            return PartialView("_TablaRol", listaRol);
        }

        //Guardar Rol
        public string Guardar(RolCLS oRolCLS, int titulo)
        {
            //Hay error
            string rpta =""; // Numero de resgistros afectados
            try { 
            if (!ModelState.IsValid) 
            {
                var query = (from state in ModelState.Values   //obetemos el estado de los errores si es que hay
                             from error in state.Errors
                             select error.ErrorMessage).ToList();
                rpta += "<ul class='list-group'>";
                foreach(var item in query)
                {
                    rpta += "<li class='list-group-item'>" + item + "</li>"; 
                }
                rpta += "</ul>";
            }
            else
            {
                using (var bd = new BDPasajeEntities())
                {
                        int cantidad = 0; // Validación duplicados (1)

                    if (titulo.Equals(-1)) // si el titulo es -1 = agregar
                    {
                        cantidad = bd.Rol.Where(p => p.NOMBRE == oRolCLS.nombre).Count();  //Validamos cuantas veces existe cantidad en la bd , Validación duplicados (2)
                        if (cantidad >= 1)
                        {
                           rpta = "-1";
                        }
                        else
                        {     
                        Rol oRol = new Rol();
                        oRol.NOMBRE = oRolCLS.nombre;
                        oRol.DESCRIPCION = oRolCLS.descripcion;
                        oRol.BHABILITADO = 1;
                        bd.Rol.Add(oRol);
                        rpta = bd.SaveChanges().ToString(); // Nos devuelve el numero de filas aceptadas
                            if (rpta == "0") rpta = ""; // Si rpta es 0 le pongo vacio
                         }
                            if (cantidad >= 1) rpta = "-1"; //-1 Ya existe en la bd, Validación duplicados (3)
                        }
                    else //Editar
                    {
                            cantidad = bd.Rol.Where(p => p.NOMBRE == oRolCLS.nombre && p.IIDROL != titulo).Count();  //Validamos cuantas veces existe cantidad en la bd , Validación duplicados (1)
                            if (cantidad >= 1)
                            {
                                rpta = "-1";
                            }
                       else
                       {

                             Rol oRol = bd.Rol.Where(p => p.IIDROL == titulo).First(); // First Devuelve solo una fila
                             oRol.NOMBRE = oRolCLS.nombre;
                             oRol.DESCRIPCION = oRolCLS.descripcion;
                             rpta = bd.SaveChanges().ToString();
                     }                                      
                  }
              }
          }
        }catch(Exception ex)
            {
                rpta = ""; // Vacio es error
            }


            return rpta;
        }

        //Recuperamos la información con Json
        public JsonResult recuperarDatos(int titulo)
        {
            RolCLS oRolCLS= new RolCLS();
            using (var bd = new BDPasajeEntities())
            {
                Rol oRol = bd.Rol.Where(p => p.IIDROL == titulo).First();
                oRolCLS.nombre = oRol.NOMBRE;
                oRolCLS.descripcion = oRol.DESCRIPCION;
            }
            return Json(oRolCLS, JsonRequestBehavior.AllowGet);
        }

        //Eliminar forma 1
        public string eliminar(RolCLS oRolCLS)
        {
            //Error
            string rpta = "";
            try
            {          
                int idrol = oRolCLS.iidRol;
                using (var bd = new BDPasajeEntities())
                {
                    Rol oRol = bd.Rol.Where(p => p.IIDROL == idrol).First();
                    oRol.BHABILITADO = 0;
                   rpta= bd.SaveChanges().ToString();
                }
            }catch(Exception ex )
            {
                //Error
               rpta = "";
            }
            return rpta;
        }
    }
}