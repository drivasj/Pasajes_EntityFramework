using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Udemy.Models;

namespace Udemy.Controllers
{
    public class ViewBagController : Controller
    {
       

        // GET: ViewBag
        BDPasajeEntities connection = new BDPasajeEntities();
        public ActionResult Index()
        {
            //ViewBag.ListaLugares = connection.SP_Lugares(idini, idfin).ToList<SP_Lugares_Result>();

            return View();
        }
        [HttpPost]
        public ActionResult Index(int idini, int idfin)
        {

            ViewBag.ListaLugares = connection.SP_Lugares(idini, idfin).ToList<SP_Lugares_Result>();
            return PartialView();
        }

        public ActionResult ViewBadDemo()
        {
            ViewBag.ListaMarca = connection.Marca.ToList<Marca>();
            ViewBag.ListaSexo = connection.Sexo.ToList<Sexo>();
            return View();
        }
    }
}