using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Udemy.Models;
using Udemy.Models.Modulos;

namespace Udemy.Controllers
{
    public class HomeController : Controller
    {
        Models.BDPasajeEntities BD = new Models.BDPasajeEntities();
        public ActionResult Index()
        {
            return View();
         //   return View(BD.SP_Marcas_Habilitadas().ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Modulos()
        {
            using (var db = new Models.Modulos.BDPasajeEntities())
            {
                //var d = db.MainMenus.Include( c=> c.).ToList();
                return View();
            }             
        }
    }
}