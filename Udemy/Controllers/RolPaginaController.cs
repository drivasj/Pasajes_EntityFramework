using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Udemy.Models;

namespace Udemy.Controllers
{
    public class RolPaginaController : Controller
    {
        // GET: RolPagina
        public ActionResult Index()
        {
            listarComboRol();
            listarComboPagina();
            List<RolPaginaCLS> listaRol = new List<RolPaginaCLS>();
            using (var bd = new BDPasajeEntities())
            {
                listaRol = (from rolpagina in bd.RolPagina
                           join rol in bd.Rol
                           on rolpagina.IIDROL equals rol.IIDROL
                           join pagina in bd.Pagina
                           on rolpagina.IIDPAGINA equals pagina.IIDPAGINA
                           where rolpagina.BHABILITADO ==1
                           select new RolPaginaCLS
                           {
                               iidrolpagina = rolpagina.IIDROLPAGINA,
                               nombreRol = rol.NOMBRE,
                               nombreMensaje = pagina.MENSAJE
                           }).ToList();
            }
                return View(listaRol);
        }

        //Filtrar
        public ActionResult Filtar(int? iidrolFiltro) //int? soporta valores nulos // Al momento de seleccionar 
        {
         
            List<RolPaginaCLS> listaRol = new List<RolPaginaCLS>();
            using (var bd = new BDPasajeEntities())
            {
                if (iidrolFiltro == null) // si es null es xq estas seleccionando la opcion seleccion al listar los combos
                {
                listaRol = (from rolpagina in bd.RolPagina
                            join rol in bd.Rol
                            on rolpagina.IIDROL equals rol.IIDROL
                            join pagina in bd.Pagina
                            on rolpagina.IIDPAGINA equals pagina.IIDPAGINA
                            where rolpagina.BHABILITADO == 1
                            select new RolPaginaCLS
                            {
                                iidrolpagina = rolpagina.IIDROLPAGINA,
                                nombreRol = rol.NOMBRE,
                                nombreMensaje = pagina.MENSAJE
                            }).ToList();
                }
                else
                {
                    listaRol = (from rolpagina in bd.RolPagina
                                join rol in bd.Rol
                                on rolpagina.IIDROL equals rol.IIDROL
                                join pagina in bd.Pagina
                                on rolpagina.IIDPAGINA equals pagina.IIDPAGINA
                                where rolpagina.BHABILITADO == 1
                                && rolpagina.IIDROL == iidrolFiltro
                                select new RolPaginaCLS
                                {
                                    iidrolpagina = rolpagina.IIDROLPAGINA,
                                    nombreRol = rol.NOMBRE,
                                    nombreMensaje = pagina.MENSAJE
                                }).ToList();
                }
            }
            return PartialView("_TableRolPagina", listaRol);
        }

        //Combo Rol
        public void listarComboRol()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from item in bd.Rol
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.NOMBRE,
                             Value = item.IIDROL.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaRol = lista;  //Paso información a la vista
            }
        }

        //Combo Pagina
        public void listarComboPagina()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from item in bd.Pagina
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.MENSAJE,
                             Value = item.IIDPAGINA.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaPagina = lista;  //Paso información a la vista
            }
        }

        //Guardar
        public string Guardar(RolPaginaCLS oRolPaginaCLS, int titulo) 
        {
            //Error
            string rpta = "";
            try
            {
        
            if (!ModelState.IsValid)  // Si el modelo no es valido 
            {
                var query = (from state in ModelState.Values   //obetemos el estado de los errores si es que hay
                             from error in state.Errors
                             select error.ErrorMessage).ToList();
                rpta += "<ul class='list-group'>";
                foreach (var item in query)
                {
                    rpta += "<li class='list-group-item'>" + item + "</li>";
                }
                rpta += "</ul>";
            }
            else  // Si el modelo  es valido 
            {
                using (var bd = new BDPasajeEntities())
                {
                    int cantidad = 0;
                    if (titulo == -1) // Agregar
                    {
                        cantidad = bd.RolPagina.Where(p => p.IIDROL == oRolPaginaCLS.iidrol 
                        && p.IIDPAGINA == oRolPaginaCLS.iidpagina).Count(); // V (1)
                        if (cantidad >= 1)
                         {
                                rpta = "-1";
                         }
                        else
                        {                                                 
                             RolPagina oRolPagina = new RolPagina();
                             oRolPagina.IIDROL = oRolPaginaCLS.iidrol;
                             oRolPagina.IIDPAGINA = oRolPaginaCLS.iidpagina;
                             oRolPagina.BHABILITADO = 1;
                             bd.RolPagina.Add(oRolPagina);
                             rpta = bd.SaveChanges().ToString();
                             if (rpta == "0") rpta = "";
                        }
                     }
                    else // Editar
                    {
                         cantidad = bd.RolPagina.Where(p => p.IIDROL == oRolPaginaCLS.iidrol 
                         && p.IIDPAGINA == oRolPaginaCLS.iidpagina
                         && p.IIDROLPAGINA!= titulo).Count(); // V (1)
                         if(cantidad >= 1)
                         {
                                rpta = "1";
                         }
                         else
                         {
                            RolPagina oRolPagina = bd.RolPagina.Where(p => p.IIDROLPAGINA == titulo).First();

                            oRolPagina.IIDROL = oRolPaginaCLS.iidrol;
                            oRolPagina.IIDPAGINA = oRolPaginaCLS.iidpagina;
                            rpta =  bd.SaveChanges().ToString();
                        }
                     }
                  }
               }
            } catch(Exception ex)
            {
                rpta = "";
            }
           return rpta;
        }

        //Recuperar informacion
        public JsonResult recuperarInformacion (int idRolPagina)
        {
            RolPaginaCLS oRolPaginaCLS = new RolPaginaCLS();
            using (var bd = new BDPasajeEntities())
            {
                RolPagina oRolPagina = bd.RolPagina.Where(p => p.IIDROLPAGINA == idRolPagina).First();
                oRolPaginaCLS.iidrol = (int)oRolPagina.IIDROL;
                oRolPaginaCLS.iidpagina =(int) oRolPagina.IIDPAGINA;
            }
            return Json(oRolPaginaCLS, JsonRequestBehavior.AllowGet);
        }

        //Eliminar
        public int EliminarRolPagina(int iidrolpagina)
        {
            int rpta = 0;
            try
            {
                using (var bd = new BDPasajeEntities())
                {
                    RolPagina oRolPagina = bd.RolPagina.Where(p => p.IIDROLPAGINA == iidrolpagina).First();
                    oRolPagina.BHABILITADO = 0;
                    rpta = bd.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                rpta = 0;
            }
            return rpta;
        }

    }
}