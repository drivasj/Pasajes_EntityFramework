using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Udemy.ClasesAuxiliares;
using Udemy.Models;

namespace Udemy.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        //CerrarSession
        public ActionResult CerrarSession()
        {
            Session["Usuario"] = null;
            Session["Rol"] = null;
            return RedirectToAction("Index");
        }
        // Iniciar Sesión
        public string Login(UsuarioCLS oUsuario)
        {
            string mensaje = "";
            

            //Si el modelo no es valido (Error)
            if (!ModelState.IsValid)
            {
                var query = (from state in ModelState.Values
                             from error in state.Errors
                             select error.ErrorMessage).ToList();
                mensaje += "<ul class='list-group'>";
                foreach (var item in query)
                {
                    mensaje += "<li class='list-group-item'>" + item + "</li>";
                }
                mensaje += "</ul>";
            }
            else
            //Bien
            {
                string nombreUsuario = oUsuario.nombreusuario;
                string password = oUsuario.contra;
                //Cifrar
                SHA256Managed sha = new SHA256Managed();
                byte[] byteContra = Encoding.Default.GetBytes(password);
                byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                //Eso es lo que cuenta
                string cadenaContraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");
                using (var bd = new BDPasajeEntities())
                {
                    int numeroVeces = bd.Usuario.Where(p => p.NOMBREUSUARIO == nombreUsuario
                    && p.CONTRA == cadenaContraCifrada).Count();
                    mensaje = numeroVeces.ToString();
                    if (mensaje == "0") mensaje = "Usuario o contraseña inconrrecta";
                    else
                    {
                        //Todo el objeto usuario
                        Usuario ousuario = bd.Usuario.Where(p => p.NOMBREUSUARIO == nombreUsuario
                       && p.CONTRA == cadenaContraCifrada).First();
                
                        //Creamos un session para mostrar en la pantalla principal el nom,bre del user que ingreso al sistema
                        Session["Usuario"] = ousuario;

                        if(ousuario.TIPOUSUARIO =="C")
                        {
                            Cliente oCliente = bd.Cliente.Where(p => p.IIDCLIENTE == ousuario.IID).First();
                            Session["nombreCompleto"] = oCliente.NOMBRE + " " + oCliente.APPATERNO + " " + oCliente.APMATERNO;
                        }
                        else
                        {
                            Empleado oEmpleado = bd.Empleado.Where(p => p.IIDEMPLEADO == ousuario.IID).First();
                            Session["nombreCompleto"] = oEmpleado.NOMBRE + " " + oEmpleado.APPATERNO + " " + oEmpleado.APMATERNO;

                        }

                        List<MenuCLS> listaMenu = (from usuario in bd.Usuario
                                                   join rol in bd.Rol
                                                   on usuario.IIDROL equals rol.IIDROL
                                                   join rolpagina in bd.RolPagina
                                                   on rol.IIDROL equals rolpagina.IIDROL
                                                   join pagina in bd.Pagina
                                                   on rolpagina.IIDPAGINA equals pagina.IIDPAGINA
                                                   where rol.IIDROL == ousuario.IIDROL && rolpagina.IIDROL == usuario.IIDROL
                                                   && usuario.IIDUSUARIO == ousuario.IIDUSUARIO
                                                   select new MenuCLS
                                                   {
                                                       nombreAccion = pagina.ACCION,
                                                       nombreControlador = pagina.CONTROLADOR,
                                                       mensaje = pagina.MENSAJE
                                                   }).ToList();

                        Session["Rol"] = listaMenu;
                    }
                }
            }

           

                return mensaje;
        }

        public string RecuperarContra(string IIDTIPO, string correo, string telfonoCelular)
        {
            string mensaje = "";
            using (var bd = new BDPasajeEntities())
            {
                int cantidad = 0;
                int iidcliente;
                if(IIDTIPO == "C")
                {
                    //Existe un cliente con esa información
                  cantidad =  bd.Cliente.Where(p => p.EMAIL == correo && p.TELEFONOCELULAR == telfonoCelular).Count();
                }
                if (cantidad == 0) mensaje = "No existe una persona registrada con esa información";
                else
                {
                    iidcliente = bd.Cliente.Where(p => p.EMAIL == correo && p.TELEFONOCELULAR == telfonoCelular).First().IIDCLIENTE;
                    //Verificar si existe el usuario
                    int nveces = bd.Usuario.Where(p => p.IID == iidcliente && p.TIPOUSUARIO == "C").Count();
                    if(nveces == 0)
                    {
                        mensaje = "No tiene usuario";
                    }
                    else
                    {
                        // Obterner su id suyo de usted jaja

                        Usuario ousuario = bd.Usuario.Where(p => p.IID == iidcliente && p.TIPOUSUARIO == "C").First();

                        //Modificar su clave
                        Random ra = new Random();
                        int n1 = ra.Next(0, 9);
                        int n2 = ra.Next(0, 9);
                        int n3 = ra.Next(0, 9);
                        int n4 = ra.Next(0, 9);

                        string nuevaContra = n1.ToString() + n2 + n3 + n4;

                        //Cifrar clave
                        SHA256Managed sha = new SHA256Managed();
                        byte[] byteContra = Encoding.Default.GetBytes(nuevaContra);
                        byte[] byteContraCifrado = sha.ComputeHash(byteContra);            
                        string cadenaContraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");

                        ousuario.CONTRA = cadenaContraCifrada;
                        mensaje = bd.SaveChanges().ToString();
                        Correo.enviarCorreo(correo, "Recuperar Clave","Su contraseña ha sido restablecida, Ahora su clave es:"+nuevaContra, "");
                    }

                   
                        
                }
            }

            return mensaje;
        }
    }
}