using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorksSpacesG9.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.IsLoggedIn = User.Identity.IsAuthenticated;

            
            if (Session["idUsuario"] != null)
            {
                ViewBag.UserId = (int)Session["idUsuario"];
            }
            else
            {
                ViewBag.UserId = null; 
            }

            return View();
        }


        public ActionResult Error()
        {
            return View();

        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}