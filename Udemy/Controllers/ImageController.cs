using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Udemy.Models;

namespace Udemy.Controllers
{
    public class ImageController : Controller
    {
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Image imageModel)
        {
           //// Image img = new Image();
           // //string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
           // //string extension = Path.GetExtension(imageModel.ImageFile.FileName);
           // //fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
           // //imageModel.ImagePath = "~/Image/" + fileName;
           // //fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
           // //imageModel.ImageFile.SaveAs(fileName);
           // //using (BDPasajeEntities db = new BDPasajeEntities())
           // {
           //     db.Image.Add(imageModel);
           //     db.SaveChanges();
           // }
           // ModelState.Clear();
            return View();
        }

        [HttpGet]
        public ActionResult View(int id)
        {
            Image imageModel = new Image();

            using (BDPasajeEntities db = new BDPasajeEntities())
            {
                imageModel = db.Image.Where(x => x.ImageID == id).FirstOrDefault();
            }

            return View(imageModel);
        }
    }
}