using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Udemy.Models;

namespace Udemy.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index(ClienteCLS oClienteCLS)
        {
            List<ClienteCLS> ListaCliente = null;
            int iidsexo = oClienteCLS.iidsexo;
            llenarSexo();
            ViewBag.lista = listaSexo; //Pasamos la lista a la vista

            //Conexion a la base de datos
            using (var bd = new BDPasajeEntities())
                if(oClienteCLS.iidsexo==0)
                {
                    ListaCliente = (from cliente in bd.Cliente
                                    where cliente.BHABILITADO == 1
                                    select new ClienteCLS
                                    {
                                        iidcliente = cliente.IIDCLIENTE,
                                        nombre = cliente.NOMBRE,
                                        appaterno = cliente.APPATERNO,
                                        apmaterno = cliente.APMATERNO,
                                        email = cliente.EMAIL,
                                        direccion = cliente.DIRECCION,
                                        telefonocelular = cliente.TELEFONOCELULAR
                                    }).ToList();
                }
                else
                {
                    ListaCliente = (from cliente in bd.Cliente
                                    where cliente.BHABILITADO == 1
                                    && cliente.IIDSEXO== iidsexo
                                    select new ClienteCLS
                                    {
                                        iidcliente = cliente.IIDCLIENTE,
                                        nombre = cliente.NOMBRE,
                                        appaterno = cliente.APPATERNO,
                                        email = cliente.EMAIL,
                                        direccion = cliente.DIRECCION,
                                        telefonocelular = cliente.TELEFONOCELULAR
                                    }).ToList();
                }
            {
                
            }
                return View(ListaCliente);
        }
        //Editar
        public ActionResult Editar(int id)
        {
            ClienteCLS oClienteCLS = new ClienteCLS();

            using (var bd = new BDPasajeEntities())
            {
                llenarSexo();
                ViewBag.lista = listaSexo; //Pasamos la lista a la vista

                Cliente oCliente = bd.Cliente.Where(p=>p.IIDCLIENTE.Equals(id)).First();
                oClienteCLS.iidcliente = oCliente.IIDCLIENTE;
                oClienteCLS.nombre = oCliente.NOMBRE;
                oClienteCLS.appaterno = oCliente.APPATERNO;
                oClienteCLS.apmaterno = oCliente.APMATERNO;
                oClienteCLS.direccion = oCliente.DIRECCION;
                oClienteCLS.email = oCliente.EMAIL;
                oClienteCLS.iidsexo = (int)oCliente.IIDSEXO;
                oClienteCLS.telefonocelular = oCliente.TELEFONOCELULAR;
                oClienteCLS.telefonofijo = oCliente.TELEFONOFIJO;

            }
                return View(oClienteCLS);
        }
        [HttpPost]
        public ActionResult Editar (ClienteCLS oClienteCLS)
        {
            int nrrgEncontrados = 0;
            int idcliente = oClienteCLS.iidcliente;
            string nombre = oClienteCLS.nombre;
            string appaterno = oClienteCLS.appaterno;
            string apmaterno = oClienteCLS.appaterno;
            using (var bd = new BDPasajeEntities())
            {
                nrrgEncontrados = bd.Cliente.Where(p => p.NOMBRE.Equals(nombre) && p.APPATERNO.Equals(appaterno) && p.APMATERNO.Equals(apmaterno) && !p.IIDCLIENTE.Equals(idcliente)).Count();
            }



                if (!ModelState.IsValid |nrrgEncontrados>=1)
                {
                if (nrrgEncontrados >= 1) oClienteCLS.mensajeError = "Ya existe el cliente";
                   llenarSexo();
                   return View(oClienteCLS);
                }
            using (var bd = new BDPasajeEntities())
            {
                Cliente oCliente = bd.Cliente.Where(p => p.IIDCLIENTE.Equals(idcliente)).First();

                oCliente.NOMBRE = oClienteCLS.nombre;
                oCliente.APPATERNO = oClienteCLS.appaterno;
                oCliente.APMATERNO = oClienteCLS.apmaterno;
                oCliente.EMAIL = oClienteCLS.email;
                oCliente.DIRECCION = oClienteCLS.direccion;
                oCliente.IIDSEXO = oClienteCLS.iidsexo;
                oCliente.TELEFONOCELULAR = oClienteCLS.telefonocelular;
                oCliente.TELEFONOFIJO = oClienteCLS.telefonofijo;

                bd.SaveChanges();
            }


            return RedirectToAction("Index");
        }

      
        //Listado de la tabla Sexo
        List<SelectListItem> listaSexo;
        private void llenarSexo()
        {
            using (var bd = new BDPasajeEntities())
            {
                listaSexo = (from sexo in bd.Sexo
                            where sexo.BHABILITADO ==1
                            select new SelectListItem
                            {
                                Text=sexo.NOMBRE,
                                Value=sexo.IIDSEXO.ToString()
                            }).ToList();
                listaSexo.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
            }
        }

        public ActionResult Agregar()
        {
            llenarSexo();
            ViewBag.lista = listaSexo; //Pasamos la lista a la vista

            return View();
        }
        [HttpPost]
        public ActionResult Agregar(ClienteCLS oClienteCLS)
        {
            int nrrgEncontrados = 0;
            string nombre = oClienteCLS.nombre;
            string appaterno = oClienteCLS.appaterno;
            string apmaterno = oClienteCLS.apmaterno;

            using (var bd = new BDPasajeEntities())
            {
                nrrgEncontrados = bd.Cliente.Where(p => p.NOMBRE.Equals(nombre) && p.APPATERNO.Equals(appaterno) 
                && p.APMATERNO.Equals(apmaterno)).Count();

            }
            ////////
            if (!ModelState.IsValid || nrrgEncontrados >=1) 
            {
                if (nrrgEncontrados >= 1) oClienteCLS.mensajeError = "Ya existe cliente registrado";
                    llenarSexo();
                    ViewBag.lista = listaSexo; 
                    return View(oClienteCLS);
            }
            using (var bd = new BDPasajeEntities())
            {
                Cliente oCliente = new Cliente();
                oCliente.NOMBRE = oClienteCLS.nombre;
                oCliente.APPATERNO = oClienteCLS.appaterno;
                oCliente.APMATERNO = oClienteCLS.apmaterno;
                oCliente.EMAIL = oClienteCLS.email;
                oCliente.DIRECCION = oClienteCLS.direccion;
                oCliente.TELEFONOCELULAR = oClienteCLS.telefonocelular;
                oCliente.IIDSEXO = oClienteCLS.iidsexo;
                oCliente.BHABILITADO = 1;
                bd.Cliente.Add(oCliente);
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Eliminar (int iidcliente)
        {
            using (var bd = new BDPasajeEntities())
            {
                Cliente oCliente= bd.Cliente.Where(p => p.IIDCLIENTE.Equals(iidcliente)).First();
                oCliente.BHABILITADO = 0;
                bd.SaveChanges();
            }

                return RedirectToAction("Index");
        }
        
     }
 }
