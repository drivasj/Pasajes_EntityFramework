using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Udemy.Models;


namespace Udemy.Controllers
{
    public class CustomersController : Controller
    {
        private readonly int _RegistrosPorPagina = 10; // Registros por pagina 

        private PaginadorGenerico<Customer> _PaginadorCustomers; // Utilizamos la clase paginador generico
        // GET: Customers
        public ActionResult Index(string buscar, int pagina = 1)
        {
            int _TotalRegistros = 0;
       
            List<Customer> listaCustomer = null;
        
            using (var bd = new BDPasajeEntities())
            {
               // string NameC = Cust.ContactName; //Busqueda Name
                // Número total de registros de la tabla Customers
                _TotalRegistros = bd.Customers.Count();

                if (buscar == null)
                {
                    listaCustomer = (from c in bd.Customers
                                     select new Customer
                                     {
                                         ContactName = c.ContactName,
                                         CompanyName = c.CompanyName,
                                         Email = c.Email

                                     }).OrderBy(x => x.ContactName)
                                 .Skip((pagina - 1) * _RegistrosPorPagina)
                                 .Take(_RegistrosPorPagina)
                                 .ToList();
                }
                else
                {
                    listaCustomer = (from c in bd.Customers
                                     where c.ContactName.Contains(buscar)
                                     select new Customer
                                     {
                                         ContactName = c.ContactName,
                                         CompanyName = c.CompanyName,
                                         Email = c.Email

                                     }).OrderBy(x => x.ContactName)
                                 .Skip((pagina - 1) * _RegistrosPorPagina)
                                 .Take(_RegistrosPorPagina)
                                 .ToList();
                }

                // Número total de páginas de la tabla Customers
                var _TotalPaginas = (int)Math.Ceiling((double)_TotalRegistros / _RegistrosPorPagina);

                // Instanciamos la 'Clase de paginación' y asignamos los nuevos valores
                _PaginadorCustomers = new PaginadorGenerico<Customer>()
                {
                    RegistrosPorPagina = _RegistrosPorPagina,
                    TotalRegistros = _TotalRegistros,
                    TotalPaginas = _TotalPaginas,
                    PaginaActual = pagina,
                    Resultado = listaCustomer
                };

            }
            return View(_PaginadorCustomers);
            // return View(listaCustomer);
        }
    }
}