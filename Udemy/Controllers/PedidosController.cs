using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Udemy.Models;


namespace Udemy.Controllers
{
    public class PedidosController : Controller
    {
        private  BDPasajeEntities mibd = new BDPasajeEntities();
        // GET: Pedidos
        public ActionResult Index()
        {
           // List<ClienteCLS>mibd.Cliente();
            return View();
        }
    }
}