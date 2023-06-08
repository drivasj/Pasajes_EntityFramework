using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Udemy.Models;
using System.Transactions;
using System.Security.Cryptography;
using System.Text;

namespace Udemy.Controllers
{
    public class UsuarioController : Controller
    {
        //Lista personas
        public void listaPersonas()
        {
            List<SelectListItem> listaPersonas = new List<SelectListItem>();

            using (var bd = new BDPasajeEntities())
            {
                //Lista de los clientes
                List<SelectListItem> listaClientes = (from item in bd.Cliente
                                                     where item.BHABILITADO == 1 && item.bTieneUsuario != 1
                                                     select new SelectListItem
                                                     {
                                                         Text = item.NOMBRE + " " + item.APPATERNO + " " + item.APMATERNO+" (C)",
                                                         Value = item.IIDCLIENTE.ToString()
                                                     }).ToList();

                // Lista de los empleados
                List<SelectListItem> listaEmpleados = (from item in bd.Empleado
                                                     where item.BHABILITADO == 1 && item.bTieneUsuario != 1
                                                     select new SelectListItem
                                                     {
                                                         Text = item.NOMBRE + " " + item.APPATERNO + " " + item.APMATERNO+" (E)",
                                                         Value = item.IIDEMPLEADO.ToString()
                                                     }).ToList();

                listaPersonas.AddRange(listaClientes);
                listaPersonas.AddRange(listaEmpleados);
                listaPersonas = listaPersonas.OrderBy(p => p.Text).ToList();
                listaPersonas.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaPersona = listaPersonas;
            }
        }
   
        //Lista Rol
        public void listarRol()
        {
            using (var bd = new BDPasajeEntities())
            {
                List<SelectListItem> listaRol;
                // Listamos los roles
                                         listaRol = (from item in bd.Rol
                                                      where item.BHABILITADO == 1
                                                      select new SelectListItem
                                                      {
                                                          Text = item.NOMBRE ,
                                                          Value = item.IIDROL.ToString()
                                                      }).ToList();
                listaRol.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaRol = listaRol;

            }
        }

        // GET: Usuarios Install-Package Microsoft.jQuery.Unobtrusive.Ajax -Version 3.2.6
        public ActionResult Index()
        {
            listaPersonas();
            listarRol();

            List<UsuarioCLS> listaUsuario = new List<UsuarioCLS>();
            using (var bd = new BDPasajeEntities())
            {
                List<UsuarioCLS> listaUsuarioCliente = (from usuario in bd.Usuario
                                                        join cliente in bd.Cliente
                                                        on usuario.IID equals
                                                        cliente.IIDCLIENTE
                                                        join rol in bd.Rol
                                                        on usuario.IIDROL equals rol.IIDROL
                                                        where usuario.bhabilitado == 1
                                                        && usuario.TIPOUSUARIO == "C"
                                                        select new UsuarioCLS
                                                        {
                                                            iidusuario = usuario.IIDUSUARIO,
                                                            nombrePersona = cliente.NOMBRE + " " + cliente.APPATERNO + " " + cliente.APMATERNO,
                                                            nombreusuario = usuario.NOMBREUSUARIO,
                                                            nombreRol = rol.NOMBRE,
                                                            nombreTipoEmpleado = ""
                                                        }).ToList();

                List<UsuarioCLS> listaUsuarioEmpleado = (from usuario in bd.Usuario
                                                        join empleado in bd.Empleado
                                                        on usuario.IID equals
                                                        empleado.IIDEMPLEADO
                                                        join rol in bd.Rol
                                                        on usuario.IIDROL equals rol.IIDROL
                                                        where usuario.bhabilitado == 1
                                                        && usuario.TIPOUSUARIO == "E"
                                                        select new UsuarioCLS
                                                        {
                                                            iidusuario = usuario.IIDUSUARIO,
                                                            nombrePersona = empleado.NOMBRE + " " + empleado.APPATERNO + " " + empleado.APMATERNO,
                                                            nombreusuario = usuario.NOMBREUSUARIO,
                                                            nombreRol = rol.NOMBRE,
                                                            nombreTipoEmpleado = ""
                                                        }).ToList();
                listaUsuario.AddRange(listaUsuarioCliente);
                listaUsuario.AddRange(listaUsuarioEmpleado);
                listaUsuario = listaUsuario.OrderBy(p => p.iidusuario).ToList();
            }

                return View(listaUsuario);
        }
        // insertar en una tabla  y actualiar un campo en otra usamos Transactions
        public string Guardar(UsuarioCLS oUsuarioCLS, int titulo)
        {
           
            string rpta = ""; // vacio es error
            try
            {
                if (!ModelState.IsValid)
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
                else {

                 using (var bd = new BDPasajeEntities())
                       {
                         int cantidad = 0;
                                        
                    using (var transaccion = new TransactionScope())
                    {
                        if (titulo.Equals(-1)) // Agregar 
                        {
                            cantidad = bd.Usuario.Where(p => p.NOMBREUSUARIO == oUsuarioCLS.nombreusuario).Count();  //V(1)
                            if(cantidad >= 1) //Validación duplicados (2)
                             {
                                  rpta = "-1";
                             }
                            else
                             { 

                            Usuario oUsuario = new Usuario();
                            oUsuario.NOMBREUSUARIO = oUsuarioCLS.nombreusuario;

                            SHA256Managed sha = new SHA256Managed();   //Cifrando contraseña                             
                            byte[] byteContra = Encoding.Default.GetBytes(oUsuarioCLS.contra);     //Convertimos una cadena a byte      
                            byte[] byteContraCifrado = sha.ComputeHash(byteContra);    //Ciframos
                            string ContraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", ""); //Convertimos el byte cifrado a cadena

                            oUsuario.CONTRA = ContraCifrada;
                            oUsuario.TIPOUSUARIO = oUsuarioCLS.nombrePersonaEnviar.Substring(oUsuarioCLS.nombrePersonaEnviar.Length - 2, 1); //oBTENEMOS EL VALOR DE LA PENULTIMA POSICION
                            oUsuario.IID = oUsuarioCLS.iid;
                            oUsuario.IIDROL = oUsuarioCLS.iidrol;
                            oUsuario.bhabilitado = 1;
                            bd.Usuario.Add(oUsuario);
                            if (oUsuario.TIPOUSUARIO.Equals("C"))
                            {
                                Cliente oCliente = bd.Cliente.Where(p => p.IIDCLIENTE.Equals(oUsuarioCLS.iid)).First();
                                oCliente.bTieneUsuario = 1;
                            }
                            else
                            {
                                Empleado oEmpleado = bd.Empleado.Where(p => p.IIDEMPLEADO.Equals(oUsuarioCLS.iid)).First();
                                oEmpleado.bTieneUsuario = 1;
                            }
                            rpta = bd.SaveChanges().ToString(); // Guardo los cambios
                                if (rpta == "0") rpta = "";
                            transaccion.Complete();
                            }
                          }
                            else // Editar
                            {
                                cantidad = bd.Usuario.Where(p => p.NOMBREUSUARIO == oUsuarioCLS.nombreusuario && p.IIDUSUARIO!= titulo).Count();  //V(1)
                                if (cantidad >= 1)
                                {
                                    rpta = "-1";
                                }
                                else
                                {                            
                                     Usuario ousuario = bd.Usuario.Where(p => p.IIDUSUARIO == titulo).First();
                                     ousuario.IIDROL = oUsuarioCLS.iidrol;
                                     ousuario.NOMBREUSUARIO = oUsuarioCLS.nombreusuario;
                                     rpta = bd.SaveChanges().ToString();
                                     transaccion.Complete();
                                }
                            }
                        }
                    }
                }
                }catch (Exception ex)                
                {
                    rpta = "";
                }
                return rpta;
            }

        //Filtrar
        public ActionResult Filtrar(UsuarioCLS oUsuarioCLS)
        {
            listaPersonas();
            listarRol();
            string nombrePersona = oUsuarioCLS.nombrePersona;

            List<UsuarioCLS> listaUsuario = new List<UsuarioCLS>();
            using (var bd = new BDPasajeEntities())
            {
                if (oUsuarioCLS.nombrePersona == null) { 
                List<UsuarioCLS> listaUsuarioCliente = (from usuario in bd.Usuario
                                                        join cliente in bd.Cliente
                                                        on usuario.IID equals
                                                        cliente.IIDCLIENTE
                                                        join rol in bd.Rol
                                                        on usuario.IIDROL equals rol.IIDROL
                                                        where usuario.bhabilitado == 1
                                                        && usuario.TIPOUSUARIO == "C"
                                                        select new UsuarioCLS
                                                        {
                                                            iidusuario = usuario.IIDUSUARIO,
                                                            nombrePersona = cliente.NOMBRE + " " + cliente.APPATERNO + " " + cliente.APMATERNO,
                                                            nombreusuario = usuario.NOMBREUSUARIO,
                                                            nombreRol = rol.NOMBRE,
                                                            nombreTipoEmpleado = "Cliente"
                                                        }).ToList();

                List<UsuarioCLS> listaUsuarioEmpleado = (from usuario in bd.Usuario
                                                         join empleado in bd.Empleado
                                                         on usuario.IID equals
                                                         empleado.IIDEMPLEADO
                                                         join rol in bd.Rol
                                                         on usuario.IIDROL equals rol.IIDROL
                                                         where usuario.bhabilitado == 1
                                                         && usuario.TIPOUSUARIO == "E"
                                                         select new UsuarioCLS
                                                         {
                                                             iidusuario = usuario.IIDUSUARIO,
                                                             nombrePersona = empleado.NOMBRE + " " + empleado.APPATERNO + " " + empleado.APMATERNO,
                                                             nombreusuario = usuario.NOMBREUSUARIO,
                                                             nombreRol = rol.NOMBRE,
                                                             nombreTipoEmpleado = "Empleado"
                                                         }).ToList();
                listaUsuario.AddRange(listaUsuarioCliente);
                listaUsuario.AddRange(listaUsuarioEmpleado);
                listaUsuario = listaUsuario.OrderBy(p => p.iidusuario).ToList();
                }
                else
                {
                    List<UsuarioCLS> listaUsuarioCliente = (from usuario in bd.Usuario
                                                            join cliente in bd.Cliente
                                                            on usuario.IID equals
                                                            cliente.IIDCLIENTE
                                                            join rol in bd.Rol
                                                            on usuario.IIDROL equals rol.IIDROL
                                                            where usuario.bhabilitado == 1
                                                               && (
                                                               cliente.NOMBRE.Contains(nombrePersona) || 
                                                               cliente.APPATERNO.Contains(nombrePersona) || 
                                                               cliente.APMATERNO.Contains(nombrePersona) 
                                                               )
                                                            && usuario.TIPOUSUARIO == "C"
                                                            select new UsuarioCLS
                                                            {
                                                                iidusuario = usuario.IIDUSUARIO,
                                                                nombrePersona = cliente.NOMBRE + " " + cliente.APPATERNO + " " + cliente.APMATERNO,
                                                                nombreusuario = usuario.NOMBREUSUARIO,
                                                                nombreRol = rol.NOMBRE,
                                                                nombreTipoEmpleado = "Cliente"
                                                            }).ToList();

                    List<UsuarioCLS> listaUsuarioEmpleado = (from usuario in bd.Usuario
                                                             join empleado in bd.Empleado
                                                             on usuario.IID equals
                                                             empleado.IIDEMPLEADO
                                                             join rol in bd.Rol
                                                             on usuario.IIDROL equals rol.IIDROL

                                                             where usuario.bhabilitado == 1
                                                               && (
                                                               empleado.NOMBRE.Contains(nombrePersona) ||
                                                               empleado.APPATERNO.Contains(nombrePersona) ||
                                                               empleado.APMATERNO.Contains(nombrePersona)
                                                               )

                                                             && usuario.TIPOUSUARIO == "E"
                                                             select new UsuarioCLS
                                                             {
                                                                 iidusuario = usuario.IIDUSUARIO,
                                                                 nombrePersona = empleado.NOMBRE + " " + empleado.APPATERNO + " " + empleado.APMATERNO,
                                                                 nombreusuario = usuario.NOMBREUSUARIO,
                                                                 nombreRol = rol.NOMBRE,
                                                                 nombreTipoEmpleado = "Empleado"
                                                             }).ToList();
                    listaUsuario.AddRange(listaUsuarioCliente);
                    listaUsuario.AddRange(listaUsuarioEmpleado);
                    listaUsuario = listaUsuario.OrderBy(p => p.iidusuario).ToList();
                }

            }

            return PartialView("_TableUsuario",listaUsuario);
        }

        //Recuperar Datos
        public JsonResult recuperarInformacion (int iidusuario)
        {
            UsuarioCLS oUsuarioCLS = new UsuarioCLS();
            using (var bd = new BDPasajeEntities())
            {
                Usuario oUsuario = bd.Usuario.Where(p => p.IIDUSUARIO == iidusuario).First();
                oUsuarioCLS.nombreusuario = oUsuario.NOMBREUSUARIO;      
                oUsuarioCLS.iidrol = (int) oUsuario.IIDROL;
            }
            return Json(oUsuarioCLS, JsonRequestBehavior.AllowGet);
        }

        //Eliminar
        public int Eliminar(int idUsuario)
        {
            int rpta = 0;
            try
            {   
                using (BDPasajeEntities bd = new BDPasajeEntities())
                {
                    Usuario oUsuario = bd.Usuario.Where(p => p.IIDUSUARIO == idUsuario).First();
                    oUsuario.bhabilitado = 0;
                    rpta = bd.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                rpta = 0; // 0 = error
            }
           
            return rpta;
        }
    }
 }
