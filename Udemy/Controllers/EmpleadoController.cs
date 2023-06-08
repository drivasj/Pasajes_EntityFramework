using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Udemy.Models;

namespace Udemy.Controllers
{
    public class EmpleadoController : Controller
    {
        // GET: Empleado
        public ActionResult Index(EmpleadoCLS oEmpleadoCls)
        {
            int idTipoUsuario = oEmpleadoCls.iidtipoUsuario;
            List<EmpleadoCLS> listaEmpleado = null;
            listarTipoUsuario();
            using (var bd = new BDPasajeEntities())
            {
                if(idTipoUsuario == 0)
                listaEmpleado = (from empleado in bd.Empleado
                                 join tipousuario in bd.TipoUsuario
                                 on empleado.IIDTIPOUSUARIO equals tipousuario.IIDTIPOUSUARIO
                                 join TipoContrato in bd.TipoContrato
                                 on empleado.IIDTIPOCONTRATO equals TipoContrato.IIDTIPOCONTRATO
                                 where empleado.BHABILITADO==1
                                 select new EmpleadoCLS
                                 {
                                     iidEmpleado = empleado.IIDEMPLEADO,
                                     nombre = empleado.NOMBRE,
                                     apPaterno = empleado.APPATERNO,
                                     apMaterno = empleado.APMATERNO,
                                     nombreTipoUsuario = tipousuario.NOMBRE,
                                     nombreTipoContrato = TipoContrato.NOMBRE,
                                     sueldo =(decimal) empleado.SUELDO
                                 }).ToList();
                else
                    listaEmpleado = (from empleado in bd.Empleado
                                     join tipousuario in bd.TipoUsuario
                                     on empleado.IIDTIPOUSUARIO equals tipousuario.IIDTIPOUSUARIO
                                     join TipoContrato in bd.TipoContrato
                                     on empleado.IIDTIPOCONTRATO equals TipoContrato.IIDTIPOCONTRATO
                                     where empleado.BHABILITADO == 1
                                     && empleado.IIDTIPOUSUARIO==idTipoUsuario
                                     select new EmpleadoCLS
                                     {
                                         iidEmpleado = empleado.IIDEMPLEADO,
                                         nombre = empleado.NOMBRE,
                                         apPaterno = empleado.APPATERNO,
                                         nombreTipoUsuario = tipousuario.NOMBRE,
                                         nombreTipoContrato = TipoContrato.NOMBRE
                                     }).ToList();

            }
                return View(listaEmpleado);
        }

        //Editar 
        public ActionResult Editar(int id)
        {
            EmpleadoCLS oEmpleadoCLS = new EmpleadoCLS();
            using (var bd = new BDPasajeEntities())
            { 
                 listarCombos();
                Empleado oEmpleado = bd.Empleado.Where(p => p.IIDEMPLEADO.Equals(id)).First();
                oEmpleadoCLS.iidEmpleado = oEmpleado.IIDEMPLEADO;
                oEmpleadoCLS.nombre = oEmpleado.NOMBRE;
                oEmpleadoCLS.apPaterno = oEmpleado.APPATERNO;
                oEmpleadoCLS.apMaterno = oEmpleado.APMATERNO;
                oEmpleadoCLS.fechaContrato = (DateTime)oEmpleado.FECHACONTRATO;
                oEmpleadoCLS.sueldo = (decimal)oEmpleado.SUELDO;
                oEmpleadoCLS.iidtipoUsuario =(int) oEmpleado.IIDTIPOUSUARIO;
                oEmpleadoCLS.iidtipoContrato = (int)oEmpleado.IIDTIPOCONTRATO;
                oEmpleadoCLS.iidSexo = (int)oEmpleado.IIDSEXO;
            }

            return View(oEmpleadoCLS);
        }

        [HttpPost]
        public ActionResult Editar(EmpleadoCLS oEmpleadoCLS)
        {
            int nrrgEncontrados = 0;
            int idEmpleado = oEmpleadoCLS.iidEmpleado;
            string nombre = oEmpleadoCLS.nombre;
            string appaterno = oEmpleadoCLS.apPaterno;
            string apmaterno = oEmpleadoCLS.apMaterno;

            using (var bd = new BDPasajeEntities())
            {
                nrrgEncontrados = bd.Empleado.Where(p => p.NOMBRE.Equals(nombre) && p.APPATERNO.Equals(appaterno) && p.APMATERNO.Equals(apmaterno) && !p.IIDEMPLEADO.Equals(idEmpleado)).Count();
            }
                if (!ModelState.IsValid || nrrgEncontrados>=1)
                {
                if (nrrgEncontrados >= 1) oEmpleadoCLS.mensajeError = "Ya existe el empleado";
                listarCombos();
                return View(oEmpleadoCLS);
                }

            using (var bd = new BDPasajeEntities())
            {
                Empleado oEmpleado = bd.Empleado.Where(p => p.IIDEMPLEADO.Equals(idEmpleado)).First();

                oEmpleado.NOMBRE = oEmpleadoCLS.nombre;
                oEmpleado.APPATERNO = oEmpleadoCLS.apPaterno;
                oEmpleado.APMATERNO = oEmpleadoCLS.apMaterno;
                oEmpleado.FECHACONTRATO = oEmpleadoCLS.fechaContrato;
                oEmpleado.SUELDO = oEmpleadoCLS.sueldo;
                oEmpleado.IIDTIPOUSUARIO = oEmpleadoCLS.iidtipoUsuario;
                oEmpleado.IIDTIPOCONTRATO = oEmpleadoCLS.iidtipoContrato;
                oEmpleado.IIDSEXO = oEmpleadoCLS.iidSexo;

                bd.SaveChanges();
            }


            return RedirectToAction("Index");
        }

        //Combo Contrato
        public void listarTipoContrato()
        {         
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from item in bd.TipoContrato
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.NOMBRE,
                             Value = item.IIDTIPOCONTRATO.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaTipoContrato = lista;  //Paso información a la vista
            }
        }

        //Combo Sexo
        public void  listarComboSexo()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from sexo in bd.Sexo
                         where sexo.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = sexo.NOMBRE,
                             Value = sexo.IIDSEXO.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaSexo = lista;  //Paso información a la vista
            }
        }

        //Combo Usuario
        public void listarTipoUsuario()
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from item in bd.TipoUsuario
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.NOMBRE,
                             Value = item.IIDTIPOUSUARIO.ToString()
                         }).ToList();
                lista.Insert(0,new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaTipoUsuario = lista;  //Paso información a la vista
            }
        }

        public void listarCombos()
        {
            listarTipoUsuario();
            listarTipoContrato();
            listarComboSexo();

        }

        public ActionResult Agregar ()
        {
            listarCombos();
            return View();
        }
        [HttpPost]
        public ActionResult Agregar(EmpleadoCLS oEmpleadoCLS)
        {
            int nrrgEncontrados = 0;
            string nombre = oEmpleadoCLS.nombre;
            string appaterno = oEmpleadoCLS.apPaterno;
            string apmaterno = oEmpleadoCLS.apMaterno;

            using (var bd=new BDPasajeEntities())
            {
                nrrgEncontrados = bd.Empleado.Where(
                    p => p.NOMBRE.Equals(nombre) && p.APPATERNO.Equals(appaterno) && p.APMATERNO.Equals(apmaterno)).Count();
                    
            }

                if (!ModelState.IsValid || nrrgEncontrados >= 1)  //Si el modelo es valido
                {
                if (nrrgEncontrados >= 1) oEmpleadoCLS.mensajeError = "El empleado ya existe";
                     listarCombos();
                    return View(oEmpleadoCLS);
                }
            using (var bd = new BDPasajeEntities())
            {
                Empleado oEmpleado = new Empleado();
                oEmpleado.NOMBRE = oEmpleadoCLS.nombre;
                oEmpleado.APPATERNO = oEmpleadoCLS.apPaterno;
                oEmpleado.APMATERNO = oEmpleadoCLS.apMaterno;

                oEmpleado.FECHACONTRATO = oEmpleadoCLS.fechaContrato;
                oEmpleado.SUELDO = oEmpleadoCLS.sueldo;
                oEmpleado.IIDTIPOUSUARIO = oEmpleadoCLS.iidtipoUsuario;
                oEmpleado.IIDTIPOCONTRATO = oEmpleadoCLS.iidtipoContrato;
                oEmpleado.IIDSEXO = oEmpleadoCLS.iidSexo;
                oEmpleado.BHABILITADO = 1;

                bd.Empleado.Add(oEmpleado);
                bd.SaveChanges();

            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Eliminar(int TextiidEmpleado)
        {
            using (var bd = new BDPasajeEntities())
            {
                Empleado oEmpleado = bd.Empleado.Where(p => p.IIDEMPLEADO.Equals(TextiidEmpleado)).First();
                oEmpleado.BHABILITADO = 0;
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }
   
    }
}