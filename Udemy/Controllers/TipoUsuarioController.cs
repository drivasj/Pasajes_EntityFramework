using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Udemy.Models;

namespace Udemy.Controllers
{
    public class TipoUsuarioController : Controller
    {
        private TipoUsuarioCLS oTipoVal;
        private bool buscarTipoUsuario(TipoUsuarioCLS oTipoUsuarioCLS)
        {
            bool BusquedaId = true;
            bool BusquedaNombre = true;
            bool BusquedaDescripcion = true;

            if (oTipoVal.iidtipousuario > 0)
                BusquedaId = oTipoUsuarioCLS.iidtipousuario.ToString().Contains(oTipoVal.iidtipousuario.ToString()); //El valor del modelo

            if (oTipoVal.nombre != null)
                BusquedaNombre = oTipoUsuarioCLS.nombre.ToString().Contains(oTipoVal.nombre);

            if (oTipoVal.descripcion != null)
                BusquedaDescripcion = oTipoUsuarioCLS.descripcion.ToString().Contains(oTipoVal.descripcion);

            return (BusquedaId && BusquedaNombre && BusquedaDescripcion);
        }

        // GET: TipoUsuario
        public ActionResult Index(TipoUsuarioCLS oTipousuario)
        {
            oTipoVal = oTipousuario;
            List<TipoUsuarioCLS> listaTipoUsuario = null;
            //var
            List<TipoUsuarioCLS> listaFiltrado;
            using (var bd =new BDPasajeEntities())
            {
                listaTipoUsuario = (from tipoUsuario in bd.TipoUsuario
                                    where tipoUsuario.BHABILITADO == 1
                                    select new TipoUsuarioCLS
                                    {
                                        iidtipousuario = tipoUsuario.IIDTIPOUSUARIO,
                                        nombre = tipoUsuario.NOMBRE,
                                        descripcion = tipoUsuario.DESCRIPCION
                                    }).ToList();
                if (oTipousuario.iidtipousuario == 0 && oTipousuario.nombre == null && oTipousuario == null
                    && oTipousuario.descripcion == null)
                    listaFiltrado = listaTipoUsuario;
                else
                {
                    Predicate<TipoUsuarioCLS> pred = new Predicate<TipoUsuarioCLS>(buscarTipoUsuario);
                    listaFiltrado = listaTipoUsuario.FindAll(pred);
                }
            }
                return View(listaFiltrado);
        }
    }
}