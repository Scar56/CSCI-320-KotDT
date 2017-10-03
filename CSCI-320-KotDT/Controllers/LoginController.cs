using System;
using System.Web.Mvc;

namespace CSCI_320_KotDT.Controllers {
    public class LoginController : Controller {
        
        [HttpGet]
        public ActionResult Login() {
            return View();
        }
        
        public ActionResult CreateUser() {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password) {
            Console.WriteLine(username);
            return RedirectToAction("Index", "Home");
        }
    }
}