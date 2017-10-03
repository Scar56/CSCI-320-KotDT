using System;
using System.Web.Mvc;

namespace CSCI_320_KotDT.Controllers {
    public class LoginController : Controller {
        
        public ActionResult Login() {
            return View();
        }
        
        public ActionResult CreateUser() {
            return View();
        }

        [HttpPost]
        public ActionResult Sign(string username, string password) {
            Console.WriteLine(username);
            return RedirectToAction("Index", "Home");
        }
    }
}