using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Udemy.Models;

namespace Udemy.Controllers
{

    public class ViajeController : Controller
    {
        // GET: Viaje
        public ActionResult Index(string cadena = "")
        {        
            List<ViajeCLS> listaViaje = null;
            listarCombos();
            using (var bd = new BDPasajeEntities())
            {
                listaViaje = (from viaje in bd.Viaje
                              join lugarOrigen in bd.Lugar
                              on viaje.IIDLUGARORIGEN equals lugarOrigen.IIDLUGAR
                              join lugarDestino in bd.Lugar
                              on viaje.IIDLUGARDESTINO equals lugarDestino.IIDLUGAR
                              join bus in bd.Bus
                              on viaje.IIDBUS equals bus.IIDBUS
                              where viaje.BHABILITADO == 1
                              select new ViajeCLS
                              {
                                  iidviaje = viaje.IIDVIAJE,
                                  nombreBus = bus.PLACA,
                                  nombreLugarOrigen = lugarOrigen.NOMBRE,
                                  nombreLugarDestino = lugarDestino.NOMBRE,
                                  precio = (decimal)viaje.PRECIO,
                                  numeroAsientosDisponibles = (int)viaje.NUMEROASIENTOSDISPONIBLES
                              }).ToList();
            }
            return View(listaViaje);
        }

        //Filtrar
        public ActionResult Filtrar (int? lugarDestinoParametro)
        {
            List<ViajeCLS> listaViaje = new List<ViajeCLS>();
            using (var bd = new BDPasajeEntities())
            {
                if(lugarDestinoParametro ==null)
                {
                    listaViaje = (from viaje in bd.Viaje
                                  join lugarOrigen in bd.Lugar
                                  on viaje.IIDLUGARORIGEN equals lugarOrigen.IIDLUGAR
                                  join lugarDestino in bd.Lugar
                                  on viaje.IIDLUGARDESTINO equals lugarDestino.IIDLUGAR
                                  join bus in bd.Bus
                                  on viaje.IIDBUS equals bus.IIDBUS
                                  where viaje.BHABILITADO == 1
                                  select new ViajeCLS
                                  {
                                      iidviaje = viaje.IIDVIAJE,
                                      nombreBus = bus.PLACA,
                                      nombreLugarOrigen = lugarOrigen.NOMBRE,
                                      nombreLugarDestino = lugarDestino.NOMBRE,
                                      precio = (decimal)viaje.PRECIO,
                                      numeroAsientosDisponibles = (int)viaje.NUMEROASIENTOSDISPONIBLES
                                  }).ToList();
                }
                else
                {
                    listaViaje = (from viaje in bd.Viaje
                                  join lugarOrigen in bd.Lugar
                                  on viaje.IIDLUGARORIGEN equals lugarOrigen.IIDLUGAR
                                  join lugarDestino in bd.Lugar
                                  on viaje.IIDLUGARDESTINO equals lugarDestino.IIDLUGAR
                                  join bus in bd.Bus
                                  on viaje.IIDBUS equals bus.IIDBUS
                                  where viaje.BHABILITADO == 1
                                  && viaje.IIDLUGARDESTINO ==(lugarDestinoParametro)
                                  select new ViajeCLS
                                  {
                                      iidviaje = viaje.IIDVIAJE,
                                      nombreBus = bus.PLACA,
                                      nombreLugarOrigen = lugarOrigen.NOMBRE,
                                      nombreLugarDestino = lugarDestino.NOMBRE,
                                      precio = (decimal)viaje.PRECIO,
                                      numeroAsientosDisponibles = (int)viaje.NUMEROASIENTOSDISPONIBLES
                                  }).ToList();
                }
            }
            return PartialView("_TablaViaje", listaViaje);
        }

        // Guardar
        public string Guardar(ViajeCLS oViajeCLS, HttpPostedFileBase foto, int titulo)
        {
            string mensaje = "";
            try
            {
                if (!ModelState.IsValid || (foto == null && titulo == -1)) // si el modelo es valido o la foto es null en el agregar ti
                { //Aqui es donde tenemos que modificar en consultas para que la imagen sea null
                    var query = (from state in ModelState.Values   //obetemos el estado de los errores si es que hay
                                 from error in state.Errors
                                 select error.ErrorMessage).ToList();
                    if (foto == null && titulo == -1) // La foto es obligatoria siempre y cuando agregues informaci{on
                    { // Si la imagen es null
                        {
                            oViajeCLS.mensaje = "La imagen es obligatoria";
                            mensaje += "<ul><li>Debe seleccionar una imagen</li></ul>";
                        }
                        mensaje += "<ul class='list-group'>";
                        foreach (var item in query)
                        {
                            mensaje += "<li class='list-group-item'>" + item + "</li>";
                        }
                        mensaje += "</ul>";
                    }
                }else
                {
                    //Leer imagen
                    byte[] fotoBD = null;
                    if(foto !=null)
                    {
                        BinaryReader lector = new BinaryReader(foto.InputStream); //Leer imagen
                        fotoBD = lector.ReadBytes((int)foto.ContentLength);
                    }
                    using (var bd = new BDPasajeEntities())
                    {
                        if(titulo == -1) // -1 = agregar
                        {
                            Viaje oViaje = new Viaje();
                            oViaje.IIDBUS = oViajeCLS.iidBus;
                            oViaje.IIDLUGARDESTINO = oViajeCLS.iidLugarDestino;
                            oViaje.IIDLUGARORIGEN = oViajeCLS.iidLugarOrigen;
                            oViaje.PRECIO = oViajeCLS.precio;
                            oViaje.FECHAVIAJE = oViajeCLS.fechaViaje;
                            oViaje.NUMEROASIENTOSDISPONIBLES = oViajeCLS.numeroAsientosDisponibles;
                            oViaje.FOTO = fotoBD;
                            oViaje.nombrefoto = oViajeCLS.nombreFoto;
                            oViaje.BHABILITADO = 1;
                            bd.Viaje.Add(oViaje);
                            mensaje = bd.SaveChanges().ToString();
                            if (mensaje == "0") mensaje = "";
                        }else
                        {
                            Viaje oViaje = bd.Viaje.Where(p => p.IIDVIAJE == titulo).First();
                            oViaje.IIDLUGARDESTINO = oViajeCLS.iidLugarDestino;
                            oViaje.IIDLUGARORIGEN = oViajeCLS.iidLugarOrigen;
                            oViaje.PRECIO = oViajeCLS.precio;
                            oViaje.FECHAVIAJE = oViajeCLS.fechaViaje;
                            oViaje.IIDBUS = oViajeCLS.iidBus;
                            oViaje.NUMEROASIENTOSDISPONIBLES = oViajeCLS.numeroAsientosDisponibles;
                            if (foto != null) oViaje.FOTO = fotoBD; // Si la imagen no es null 
                            mensaje = bd.SaveChanges().ToString();


                        }
                    }               
                }

            }catch (Exception ex)
            {
                mensaje = "";
            }
            return mensaje;
        }

      //  Recuperar información
        public JsonResult recuperarInformacion(int idViaje)
        {
            ViajeCLS oViajeCLS = new ViajeCLS();
            using (var bd = new BDPasajeEntities())
            {
                Viaje oViaje = bd.Viaje.Where(p => p.IIDVIAJE == idViaje).First();
                oViajeCLS.iidviaje = oViaje.IIDVIAJE;
                oViajeCLS.iidBus = (int)oViaje.IIDBUS;
                oViajeCLS.iidLugarDestino = (int)oViaje.IIDLUGARDESTINO;
                oViajeCLS.iidLugarOrigen = (int)oViaje.IIDLUGARORIGEN;
                oViajeCLS.precio = (decimal)oViaje.PRECIO;
                //año-mes-dia (pide)
                //bd (dia-mes-año)
                oViajeCLS.fechaViajeCadena = oViaje.FECHAVIAJE !=null ? ((DateTime)oViaje.FECHAVIAJE).ToString("yyy-MM-ddTHH:mm") : ""; //
                oViajeCLS.numeroAsientosDisponibles = (int)oViaje.NUMEROASIENTOSDISPONIBLES;
                oViajeCLS.nombreFoto = oViaje.nombrefoto;

                // pARA MOSTRATR LA FOTO DEBES SABER LA EXTENSDIOm
                oViajeCLS.extension = Path.GetExtension(oViaje.nombrefoto); // De este forma mostramos la extension
                oViajeCLS.fotoRecuperCadena = Convert.ToBase64String(oViaje.FOTO); //Recuperar la foto ~byte
            }
            return Json(oViajeCLS, JsonRequestBehavior.AllowGet);
        }

        public void listarBus()   // Lista nombre tabla bus
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
                ViewBag.listaBus = lista;  //Paso información a la vista
            }
        }

        public void listarLugar() // Lista nombre tabla Lugar
        {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities())
            {
                lista = (from item in bd.Lugar
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {
                             Text = item.NOMBRE,
                             Value = item.IIDLUGAR.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaLugar = lista;  //Paso información a la vista
            }
        }

        public void listarCombos()
        {
            listarLugar();
            listarBus();
        }

        public int EliminarViaje (int idViaje)
        {
            int nregistrosAfectados = 0;
            try
            {
                using (var bd = new BDPasajeEntities())
                {
                    Viaje oViaje = bd.Viaje.Where(p => p.IIDVIAJE == idViaje).First();
                    oViaje.BHABILITADO = 0;
                    nregistrosAfectados = bd.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                nregistrosAfectados = 0;
            }

            return nregistrosAfectados; 
        }

    }
}