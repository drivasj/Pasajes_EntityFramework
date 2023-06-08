using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Udemy.Models;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using System.Collections;
using System.Text;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Udemy.Filters;

namespace Udemy.Controllers
{
    [Acceder] // Seguridad
    public class MarcaController : Controller
    {
        //Generar Reporte Excel libreria EPPlus 4.5.3.1
        public FileResult GenerarExcel()
        {
            byte[] buffer;

            using (MemoryStream ms = new MemoryStream())
            {
                // Todo el documento excel
            //  ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage ep = new ExcelPackage();
                //Crear una hoja
                ep.Workbook.Worksheets.Add("Reporte");

                ExcelWorksheet ew = ep.Workbook.Worksheets[1];

                //Ponemos nombre de las columnas

                ew.Cells[1, 1].Value = "ID Marca";
                ew.Cells[1, 2].Value = "Nombre";
                ew.Cells[1, 3].Value = "Descripción";

                //Acho de cada columna

                ew.Column(1).Width = 20;
                ew.Column(2).Width = 40;
                ew.Column(3).Width = 60;

                //Estilo de la cabecera 

                using (var range = ew.Cells[1, 1, 1, 3])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid; //Estilo
                    range.Style.Font.Color.SetColor(Color.Black); //Color de fondo
                    range.Style.Fill.BackgroundColor.SetColor(Color.Violet); // Color de letra
                }
                List<MarcaCLS> lista = (List<MarcaCLS>)Session["lista"];
                    int nregistros = lista.Count;

                //FOR
                for(int i = 0; i < nregistros; i++ )
                {//[i + 2da Fila, 1ra Columna]
                    ew.Cells[i + 2, 1].Value = lista[i].iidmarca;
                    ew.Cells[i + 2, 2].Value = lista[i].nombre;
                    ew.Cells[i + 2, 3].Value = lista[i].descripcion;


                }
                //Guardar Excel
                ep.SaveAs(ms);
                buffer = ms.ToArray();

            }
            return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
      
        //Generar Reporte PDF libreria itexsharp
        public FileResult GenerarPDF()
        {
            Document doc = new Document(); //Creamos un documento
            byte[] buffer;  //Creo un array de buffer
            
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter.GetInstance(doc, ms); //Guardamos el documento crado en memoria 
                doc.Open(); //Abrimos el documento

                Paragraph title = new Paragraph("Lista Marca");
                title.Alignment = Element.ALIGN_CENTER; //Titulo centrado
                doc.Add(title); // Agregar titulo

                Paragraph espacio = new Paragraph("  "); // Agregamos un espacio

                //Columnas(Tabla)
                PdfPTable table = new PdfPTable(3); //Tabla3x3
                float[] values = new float[3] { 30, 40, 80 };  // Columnas y Ancho
                table.SetTotalWidth(values); // Asignamos el ancho a las tablas 

                //Creando celdas #1
                PdfPCell celda1  = new PdfPCell(new Phrase("ID MARCA"));
                celda1.BackgroundColor = new BaseColor(130, 130, 130); //Color de la celda 
                celda1.HorizontalAlignment = PdfPCell.ALIGN_CENTER; //  la Primera celda alineada 
                table.AddCell(celda1); //Agregar celda1

                // Celda #2
                PdfPCell celda2 = new PdfPCell(new Phrase("NOMBRE"));
                celda2.BackgroundColor = new BaseColor(130, 130, 130); //Color de la celda 
                celda2.HorizontalAlignment = PdfPCell.ALIGN_CENTER; //  la Primera celda alineada 
                table.AddCell(celda2); //Agregar celda2

                // Celda #3
                PdfPCell celda3 = new PdfPCell(new Phrase("DESCRIPCIÓN"));
                celda3.BackgroundColor = new BaseColor(130, 130, 130); //Color de la celda 
                celda3.HorizontalAlignment = PdfPCell.ALIGN_CENTER; //  la Primera celda alineada 
                table.AddCell(celda3); //Agregar celda3

               List<MarcaCLS> lista = (List<MarcaCLS>)Session["lista"];
                int nregistros = lista.Count;
                for (int i = 0; i < nregistros; i++)
                {
                    table.AddCell(lista[i].iidmarca.ToString());
                    table.AddCell(lista[i].nombre.ToString());
                    table.AddCell(lista[i].descripcion.ToString());
                }

                doc.Add(table);

                //Columnas(Tabla)
                PdfPTable table2 = new PdfPTable(3); //Tabla3x3
                float[] values2 = new float[3] { 30, 40, 80 };  // Columnas y Ancho
                table2.SetTotalWidth(values2); // Asignamos el ancho a las tablas 

                //Creando celdas #1
                PdfPCell celda4 = new PdfPCell(new Phrase("ID MARCA2"));
                celda4.BackgroundColor = new BaseColor(130, 130, 130); //Color de la celda 
                celda4.HorizontalAlignment = PdfPCell.ALIGN_CENTER; //  la Primera celda alineada 
                table2.AddCell(celda4); //Agregar celda4

                // Celda #2
                PdfPCell celda5 = new PdfPCell(new Phrase("NOMBRE2"));
                celda5.BackgroundColor = new BaseColor(130, 130, 130); //Color de la celda 
                celda5.HorizontalAlignment = PdfPCell.ALIGN_CENTER; //  la Primera celda alineada 
                table2.AddCell(celda5); //Agregar celda5

                // Celda #3
                PdfPCell celda6 = new PdfPCell(new Phrase("DESCRIPCIÓN2"));
                celda6.BackgroundColor = new BaseColor(130, 130, 130); //Color de la celda 
                celda6.HorizontalAlignment = PdfPCell.ALIGN_CENTER; //  la Primera celda alineada 
                table2.AddCell(celda6); //Agregar celda6

                doc.Close();



                buffer = ms.ToArray(); //Tenemos el PDF en la variable buffer
            }
            return File(buffer,"application/pdf",("Listado.pdf")); // application/pdf =ContentType(nombre.pdf)
        }

        // GET: Marca
        public ActionResult Index(MarcaCLS omarcaCLS)
        {  
            //Nos conectamos a la base de datos con EntityFramework
            List<MarcaCLS> listaMarca = null; // La lista se retorna afuera del using para poder retornar en la vista
            using (var bd = new BDPasajeEntities())       
            {
                string nombreMarca = omarcaCLS.nombre; //Busqueda nombre
                if (omarcaCLS.nombre == null)
                {
                    // Listado de la tabla Marca SELECT * FROM Marca
                    // Listado de la tabla Marca SELECT * FROM Marca
                    listaMarca = (from marca in bd.Marca
                                  where marca.BHABILITADO == 1
                                  select new MarcaCLS
                                  {
                                      //////iidmarca = marca.IIDMARCA,
                                      nombre = marca.NOMBRE,
                                      descripcion = marca.DESCRIPCION
                                  }).ToList();
                    Session["lista"] = listaMarca; //Creamos una variable super global
                } 
                else
                {
                    listaMarca = (from marca in bd.Marca
                                  where marca.BHABILITADO == 1
                                  && marca.NOMBRE.Contains(nombreMarca)
                                  select new MarcaCLS
                                  {
                                      iidmarca = marca.IIDMARCA,
                                      nombre = marca.NOMBRE,
                                      descripcion = marca.DESCRIPCION
                                  }).ToList();
                    Session["lista"] = listaMarca; //Creamos una variable super global

                }
            }
            return View(listaMarca);
        }

        public ActionResult Agregar() // Muestra la vista
        {
            return View();
        }
        //Editar
    
        public ActionResult Editar(int id)
        {
            MarcaCLS oMarcaCLS = new MarcaCLS();
            using (var bd = new BDPasajeEntities())
            {
                Marca oMarca = bd.Marca.Where(p => p.IIDMARCA.Equals(id)).First();
                oMarcaCLS.iidmarca = oMarca.IIDMARCA;
                oMarcaCLS.nombre = oMarca.NOMBRE;
                oMarcaCLS.descripcion = oMarca.DESCRIPCION;
            }
            return View(oMarcaCLS);
        }

        [HttpPost]
        public ActionResult Editar(MarcaCLS oMarcaCLS, MarcaDRCLS oMarcaDRCLS, ContadorMarcaCLS CMCLS)
        {
            int nrrgEncontrados = 0; // var Resgistros encontrados 
            int nrcontador = 0; //CONTADOR DE RESGITROS CONTADOR MARCA ----1
            string nombreMarca = oMarcaCLS.nombre;
            // int iidmarca = oMarcaCLS.iidmarca;
            string IDMC = CMCLS.iidmarca;   //VAR CONTADOR MARCA  ----2
            using (var bd = new BDPasajeEntities())
            {
              // nrrgEncontrados = busqueda marca donde el nombre = nombreMarca y IIDMARCA
             //   nrrgEncontrados = bd.Marca.Where(p => p.NOMBRE.Equals(nombreMarca) && !p.IIDMARCA.Equals(iidmarca)).Count(); // y que sea diferente al mismo id
                nrcontador = bd.ContadorMarca.Where(p => p.IIDMARCA.Equals(IDMC)).Count(); // ----3
            }
                if (!ModelState.IsValid || nrcontador >= 1)  //----4 // Si el modelo no es valido o el numero de registros es >=1
                {
                    //  if  (nrrgEncontrados >= 1) oMarcaCLS.mensajeError = "El nombre marca ya existe"; // Pasamos a la vista agregar el mensaje si se editar una marca repetida
                     // return View(oMarcaCLS);
                      if (nrcontador >= 1) oMarcaCLS.mensajeError = "El nombre marca ya existe"; //
                      return View(oMarcaCLS);
            }


            int idMarca = oMarcaCLS.iidmarca; //Recuperamos el registro de la bd
            using (var bd = new BDPasajeEntities())
            {
                Marca oMarca = bd.Marca.Where(p => p.IIDMARCA.Equals(idMarca)).First();
                oMarca.NOMBRE = oMarcaCLS.nombre;
                oMarca.DESCRIPCION = oMarcaCLS.descripcion;
                //---
                MarcaDR oMarcaDR = new MarcaDR();
                oMarcaDR.NOMBRE = oMarcaCLS.nombre;
                oMarcaDR.DESCRIPCION = oMarcaCLS.descripcion;
                oMarcaDR.BHABILITADO = 1;
                bd.MarcaDR.Add(oMarcaDR);
                bd.SaveChanges();
                //-- 
               ContadorMarca CM = new ContadorMarca();
               CM.IIDMARCA = oMarcaCLS.iidmarca.ToString();
                bd.ContadorMarca.Add(CM);
                bd.SaveChanges();
            }


                return RedirectToAction("Index");
        }
   
        //Agregar
        [HttpPost] //Hace la inserción 
        public ActionResult Agregar(MarcaCLS oMarcaCLS, MarcaDRCLS oMarcaDRCLS)
        {
            int nrrgEncontrados = 0;
            string nombreMarca = oMarcaCLS.nombre;
            using (var bd = new BDPasajeEntities())
            {
                nrrgEncontrados = bd.Marca.Where(p => p.NOMBRE.Equals(nombreMarca)).Count();
            }
              //////
                if (!ModelState.IsValid || nrrgEncontrados >=1)
                {
                if (nrrgEncontrados >= 1) oMarcaCLS.mensajeError = "El nombre marca ya existe"; // Pasamos a la vista agregar el mensaje si se ingresa una marca repetida
                    return View(oMarcaCLS); //Decuelve toda la vista 
                }
                else
                {
                    using (var bd = new BDPasajeEntities())
                    {
                        //1ra inserción 
                        Marca oMarca = new Marca();
                        oMarca.NOMBRE = oMarcaCLS.nombre;
                        oMarca.DESCRIPCION = oMarcaCLS.descripcion;
                        oMarca.BHABILITADO = 1;
                        //2da inserción 
                        MarcaDR oMarcaDR = new MarcaDR();
                        oMarcaDR.NOMBRE = oMarcaDRCLS.nombre;
                        oMarcaDR.DESCRIPCION = oMarcaDRCLS.descripcion;
                        oMarcaDR.BHABILITADO = 1;

                        bd.Marca.Add(oMarca);  //1
                        bd.MarcaDR.Add(oMarcaDR);  //2
                        bd.SaveChanges();
                    }
                }
            return RedirectToAction("Index");
        }

        // inhabilita no elimina 
        public ActionResult Eliminar(int id)
        {
            using (var bd=new BDPasajeEntities())
            {
                Marca oMarca = bd.Marca.Where(p => p.IIDMARCA.Equals(id)).First();
                oMarca.BHABILITADO = 0;  // 0 inhabilitado 1 Habilitado
                bd.SaveChanges();
            }
                return RedirectToAction("Index");
        }
        
    }
}