using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Udemy.Models;


namespace Udemy.Filters
{
    
    public class Acceder:ActionFilterAttribute
    {
        //Antes que tu llames a una URL va a pasar por este metodo
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Si session es nulo, entonces que te retorne a login
            var usuario = HttpContext.Current.Session["Usuario"];

            List<MenuCLS> roles = (List<MenuCLS>)HttpContext.Current.Session["Rol"];

            string nombreControlador = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName; // Sacamos el nombre del controlador
            string accion = filterContext.ActionDescriptor.ActionName; // Sacamos el nombre de la accion
            int cantidad = roles.Where(p => p.nombreControlador == nombreControlador).Count();

            if(usuario == null || cantidad==0)
            {
                filterContext.Result = new RedirectResult("~/Login/Index");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}