using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Udemy.Models;


namespace Udemy.Controllers
{
    public class BusController : Controller
    {
        // GET: Bus
        public ActionResult Index(BusCLS oBusCls)
        {
            listarCombos();
            List<BusCLS> ListaRpta = new List<BusCLS>();// 
            List<BusCLS> listaBus = null;
            using (var bd = new BDPasajeEntities())
            {
                listaBus = (from bus in bd.Bus
                            join sucursal in bd.Sucursal
                            on bus.IIDSUCURSAL equals sucursal.IIDSUCURSAL
                            join tipoBus in bd.TipoBus
                            on bus.IIDTIPOBUS equals tipoBus.IIDTIPOBUS
                            join tipoModelo in bd.Modelo
                            on bus.IIDMODELO equals tipoModelo.IIDMODELO
                            where bus.BHABILITADO == 1
                            select new BusCLS
                            {
                                iidBus = bus.IIDBUS,
                                placa = bus.PLACA,
                                nombreModelo = tipoModelo.NOMBRE,
                                nombreSucursal = sucursal.NOMBRE,
                                nombreTipoBus = tipoBus.NOMBRE,
                                iidModelo = tipoModelo.IIDMODELO,
                                iidSucursal = sucursal.IIDSUCURSAL,
                                iidTipoBus = tipoBus.IIDTIPOBUS,
                                fechaCompra = (DateTime)bus.FECHACOMPRA
                            }).ToList();

                if (oBusCls.iidBus == 0 && oBusCls.placa == null
                  && oBusCls.iidModelo == 0 && oBusCls.iidSucursal == 0
                  && oBusCls.iidTipoBus == 0)
                {
                    ListaRpta = listaBus;
                }
                else
                {
                    //Filtro por Bus
                    if (oBusCls.iidBus != 0)
                    {
                        listaBus = listaBus.Where(p => p.iidBus.ToString().Contains(oBusCls.iidBus.ToString())).ToList();
                    }
                    //Filtro por Placa

                    if (oBusCls.placa != null)
                    {
                        listaBus = listaBus.Where(p => p.placa.Contains(oBusCls.placa)).ToList();

                    }

                    //Filtro por Modelo

                    if (oBusCls.iidModelo != 0)
                    {
                        listaBus = listaBus.Where(p => p.iidModelo.ToString().Contains(oBusCls.iidModelo.ToString())).ToList();

                    }

                    //Filtro por Sucursak

                    if (oBusCls.iidSucursal != 0)
                    {
                        listaBus = listaBus.Where(p => p.iidSucursal.ToString().Contains(oBusCls.iidSucursal.ToString())).ToList();

                    }

                    //Filtro por Tipo Bus

                    if (oBusCls.iidTipoBus != 0)
                    {
                        listaBus = listaBus.Where(p => p.iidTipoBus.ToString().Contains(oBusCls.iidTipoBus.ToString())).ToList();

                    }
                    ListaRpta = listaBus;
                }

            }
            return View(ListaRpta);
        }

        public ActionResult Agregar()
        {
            listarCombos();
            return View();
        }

        //Insertar 
        [HttpPost]
        public ActionResult Agregar(BusCLS oBusCls)
        {
            int nrrgEncontrados = 0;
            string placa = oBusCls.placa; //Obtenemos el numero de placa 

            using (var bd = new BDPasajeEntities())
            {
                nrrgEncontrados = bd.Bus.Where(p => p.PLACA.Equals(placa)).Count();
            }

            if (!ModelState.IsValid || nrrgEncontrados >= 1)
            {
                if (nrrgEncontrados >= 1) oBusCls.mensajeError = "Ya existe el bus";
                listarCombos();
                return View(oBusCls);
            }

            using (var bd = new BDPasajeEntities())
            {

                if (!ModelState.IsValid)
                {
                    listarCombos();
                    return View(oBusCls);
                }
                Bus oBus = new Bus();
                oBus.IIDSUCURSAL = oBusCls.iidSucursal;
                oBus.IIDTIPOBUS = oBusCls.iidTipoBus;
                oBus.PLACA = oBusCls.placa;
                oBus.FECHACOMPRA = oBusCls.fechaCompra;
                oBus.IIDMODELO = oBusCls.iidModelo;

                oBus.NUMEROFILAS = oBusCls.numeroFilas;
                oBus.NUMEROCOLUMNAS = oBusCls.numeroColumnas;

                oBus.DESCRIPCION = oBusCls.descripcion;
                oBus.OBSERVACION = oBusCls.observacion;
                oBus.IIDMARCA = oBusCls.iidmarca;
                oBus.BHABILITADO = 1;
                bd.Bus.Add(oBus);
                bd.SaveChanges();


            }
              
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Editar(BusCLS oBusCLS)
        {
            int nregistrosEncontrados = 0;
            int idBus = oBusCLS.iidBus;
            string placa = oBusCLS.placa;

            using (var bd = new BDPasajeEntities())
            {
                nregistrosEncontrados = bd.Bus.Where(p => p.PLACA.Equals(placa) && !p.IIDBUS.Equals(idBus)).Count();
            }



            if (!ModelState.IsValid || nregistrosEncontrados >= 1)
            {
                if (nregistrosEncontrados >= 1) oBusCLS.mensajeError = "El bus ya existe";
                listarCombos();
                return View(oBusCLS);
            }
            using (var bd = new BDPasajeEntities())
            {
                Bus oBus = bd.Bus.Where(p => p.IIDBUS.Equals(idBus)).First();

                oBus.IIDSUCURSAL = oBusCLS.iidSucursal;
                oBus.IIDTIPOBUS = oBusCLS.iidTipoBus;
                oBus.PLACA = oBusCLS.placa;
                oBus.FECHACOMPRA = oBusCLS.fechaCompra;
                oBus.IIDMODELO = oBusCLS.iidModelo;
                oBus.NUMEROCOLUMNAS = oBusCLS.numeroColumnas;
                oBus.NUMEROFILAS = oBusCLS.numeroFilas;
                oBus.DESCRIPCION = oBusCLS.descripcion;
                oBus.OBSERVACION = oBusCLS.observacion;
                oBus.IIDMARCA = oBusCLS.iidmarca;

                bd.SaveChanges();

            }
            return RedirectToAction("Index"); //Return -> Si el editar esta bien 
        }

    public ActionResult Editar(int id)
        {
            BusCLS oBusCLS = new BusCLS();
            listarCombos();
            using (var bd = new BDPasajeEntities())
            {
                Bus obus = bd.Bus.Where(p => p.IIDBUS.Equals(id)).First();
                oBusCLS.iidBus = obus.IIDBUS;
                oBusCLS.iidSucursal =(int) obus.IIDSUCURSAL;
                oBusCLS.iidTipoBus = (int)obus.IIDTIPOBUS;
                oBusCLS.placa = obus.PLACA;
                oBusCLS.fechaCompra = (DateTime) obus.FECHACOMPRA;
                oBusCLS.iidModelo = (int)obus.IIDMODELO;
                oBusCLS.numeroColumnas = (int)obus.NUMEROCOLUMNAS;
                oBusCLS.numeroFilas = (int)obus.NUMEROFILAS;
                oBusCLS.descripcion = obus.DESCRIPCION;
                oBusCLS.observacion = obus.OBSERVACION;
                oBusCLS.iidmarca = (int)obus.IIDMARCA;
            }


            return View();
        }

      


        public void listarTipoBus()
        {
            //agregar
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from item in bd.TipoBus
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.NOMBRE,
                             Value = item.IIDTIPOBUS.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaTipoBus = lista;
            }
        }

        public void listarMarca()
        {
            //agregar
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from item in bd.Marca
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.NOMBRE,
                             Value = item.IIDMARCA.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaMarca = lista;
            }
        }


        public void listarModelo()
        {
            //agregar
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from item in bd.Modelo
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.NOMBRE,
                             Value = item.IIDMODELO.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaModelo = lista;
            }
        }

        public void listarSucursal()
        {
            //agregar
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from item in bd.Sucursal
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.NOMBRE,
                             Value = item.IIDSUCURSAL.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaSucursal = lista;
            }
        }


        public void listarCombos()
        {
            listarSucursal();
            listarModelo();
            listarMarca();
            listarTipoBus();
        }
        [HttpPost]
        public ActionResult Eliminar(int iidBus)
        {
            using (var bd = new BDPasajeEntities())
            {
                Bus oBus = bd.Bus.Where(p => p.IIDBUS.Equals(iidBus)).First();
                oBus.BHABILITADO = 0;
                bd.SaveChanges();
            }
                return RedirectToAction("Index");
        }

    }
}