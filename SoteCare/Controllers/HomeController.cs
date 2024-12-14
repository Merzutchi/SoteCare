
using SoteCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoteCare.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.LoginError = 0; // Ei virhettä
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

        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(Users LoginModel)
        {
            PatientRecordDataEntities db = new PatientRecordDataEntities();
            
            var LoggedUser = db.Users.SingleOrDefault(x => x.Username == LoginModel.Username && x.Password == LoginModel.Password);
            if (LoggedUser != null)
            {
                ViewBag.LoginMessage = "Kirjautuminen onnistui";
                ViewBag.LoggedStatus = "In";
                ViewBag.LoginError = 0; // Ei virhettä
                Session["UserName"] = LoggedUser.Username;
                return RedirectToAction("Index", "Home"); //Tässä määritellään mihin onnistunut kirjautuminen johtaa --> Home/Index
            }
            else
            {
                ViewBag.LoginMessage = "Kirjautuminen epäonnistui";
                ViewBag.LoggedStatus = "Out";
                ViewBag.LoginError = 1; // Virhe
                LoginModel.LoginErrorMessage = "Tuntematon käyttäjätunnus tai salasana.";
                return View("Index", LoginModel);
            }

        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            ViewBag.LoggedStatus = "Out";
            return RedirectToAction("Index", "Home"); // Uloskirjauduttua jälleen pääsivulle
        }

    }
}
