using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Udemy.Models;

namespace Udemy.Controllers
{
    public class ReservasRealizadasController : Controller
    {
        // GET: ReservasRealizadas
        public ActionResult Index()
        {
            // Obtener el id del usuario logueado
            Usuario ousuario = (Usuario)Session["Usuario"];
            int iidusuario = ousuario.IIDUSUARIO;

            List<ReservasRealizadasCLS> listaReserva = new List<ReservasRealizadasCLS>();

            using (BDPasajeEntities db = new BDPasajeEntities())
            {
                listaReserva = (from venta in db.VENTA
                                where venta.BHABILITADO == 1
                                && venta.IIDUSUARIO == iidusuario
                                select new ReservasRealizadasCLS
                                {
                                    iidventa = venta.IIDVENTA,
                                    fechaVenta = (DateTime)venta.FECHAVENTA,
                                    total = (decimal)venta.TOTAL
                                }).ToList();
            }


                return View(listaReserva);
        }
    }
}