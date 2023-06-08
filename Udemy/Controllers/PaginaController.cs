using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Udemy.Models;

namespace Udemy.Controllers
{
    public class PaginaController : Controller
    {
        // GET: Pagina Index
        public ActionResult Index()
        {
            List<PaginaCLS> listaPagina = new List<PaginaCLS>();
            using (var bd = new BDPasajeEntities())
            {
                listaPagina = (from pagina in bd.Pagina
                               where pagina.BHABILITADO == 1
                               select new PaginaCLS
                               {
                                   iidpagina = pagina.IIDPAGINA,
                                   mensaje = pagina.MENSAJE,
                                   controlador = pagina.CONTROLADOR,
                                   accion = pagina.ACCION
                               }).ToList();
            }
                return View(listaPagina);
        }

        //Filtrar
        public ActionResult Filtrar(PaginaCLS oPaginaCLS)
        {
            string mensajeFiltro = oPaginaCLS.mensajeFiltro;

            List<PaginaCLS> listaPagina = new List<PaginaCLS>();

            using (var bd = new BDPasajeEntities())
            {
                if (mensajeFiltro == null)
                {
                    listaPagina = (from pagina in bd.Pagina
                                   where pagina.BHABILITADO == 1
                                   select new PaginaCLS
                                   {
                                       iidpagina = pagina.IIDPAGINA,
                                       mensaje = pagina.MENSAJE,
                                       controlador = pagina.CONTROLADOR,
                                       accion = pagina.ACCION
                                   }).ToList();
                }
                else
                {
                    listaPagina = (from pagina in bd.Pagina
                                   where pagina.BHABILITADO == 1
                                   && pagina.MENSAJE.Contains(mensajeFiltro)
                                   select new PaginaCLS
                                   {
                                       iidpagina = pagina.IIDPAGINA,
                                       mensaje = pagina.MENSAJE,
                                       controlador = pagina.CONTROLADOR,
                                       accion = pagina.ACCION
                                   }).ToList();
                }
                
            }
            return PartialView("_TablaPagina", listaPagina);
        }

        //Guardar
        public string Guardar(PaginaCLS oPaginaCLS, int titulo)
        {         
            string rpta  =""; //Error
            try {
                if (!ModelState.IsValid)  // SI el modelo no es valido 
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
                else
                {            
            using (var bd = new BDPasajeEntities())
            {
                int cantidad = 0;
                 
                if (titulo == -1) //Agregar
                {
                    cantidad = bd.Pagina.Where(p => p.MENSAJE == oPaginaCLS.mensaje).Count(); // V1
                    if (cantidad >=1) //v2
                    {
                        rpta = "-1";
                    }
                    else
                    {
                         Pagina oPagina = new Pagina();
                         oPagina.MENSAJE = oPaginaCLS.mensaje;
                         oPagina.ACCION = oPaginaCLS.accion;
                         oPagina.CONTROLADOR = oPaginaCLS.controlador;
                         oPagina.BHABILITADO = 1;
                         bd.Pagina.Add(oPagina);
                         rpta = bd.SaveChanges().ToString();
                         if (rpta == "0") rpta = "";
                    }

                  
                } else  //Editar
                    {
                        cantidad = bd.Pagina.Where(p => p.MENSAJE == oPaginaCLS.mensaje && p.IIDPAGINA != titulo).Count(); // V1
                        if(cantidad >=1)
                        {
                                rpta = "-1";
                        }
                        else
                        {
                             Pagina oPagina = bd.Pagina.Where(p => p.IIDPAGINA == titulo).First();
                             oPagina.MENSAJE = oPaginaCLS.mensaje;
                             oPaginaCLS.controlador = oPaginaCLS.controlador;
                             oPaginaCLS.accion = oPaginaCLS.accion;
                             rpta = bd.SaveChanges().ToString();
                       }
                    }
                  }
                }
              }
            catch(Exception ex)
            {
                rpta = "";
            }
            return rpta;
        }

        //Recuperar inf
        public JsonResult recuperarInformacion(int idPagina)
        {
            PaginaCLS oPaginaCLS = new PaginaCLS();
            using (var bd = new BDPasajeEntities())
            {
                Pagina oPagina = bd.Pagina.Where(p => p.IIDPAGINA == idPagina).First(); //First solo nos devulve una fila
                oPaginaCLS.mensaje = oPagina.MENSAJE;
                oPaginaCLS.accion = oPagina.ACCION;
                oPaginaCLS.controlador = oPagina.CONTROLADOR;
            }
            return Json(oPaginaCLS, JsonRequestBehavior.AllowGet);
        }

        //Eliminar
        public int EliminarPagina(int iidpagina)
        {
            int rpta = 0;
            try
            {
                using (var bd = new BDPasajeEntities())
                {
                    Pagina oPagina = bd.Pagina.Where(p => p.IIDPAGINA == iidpagina).First();
                    oPagina.BHABILITADO = 0;
                    rpta = bd.SaveChanges();
                }
            }catch(Exception ex)
            {
                rpta = 0;
            }
            return rpta;
        }
    }
 }
