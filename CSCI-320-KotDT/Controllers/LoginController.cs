using System;
using System.Data.SqlClient;
using System.Web.Mvc;
using Npgsql;

namespace CSCI_320_KotDT.Controllers {
    public class LoginController : Controller {
        
        [HttpGet]
        public ActionResult Login() {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password) {
            Console.WriteLine(username);
            return RedirectToAction("Index", "Home");
        }
    
        
        [HttpGet]
        public ActionResult CreateUser() {
            ViewBag.invalid = false;
            return View();
        }
        [HttpPost]
        public ActionResult CreateUser(string username, string password, string firstName, string lastName) {
            //check validity
            using(var conn = new NpgsqlConnection("Host=reddwarf.cs.rit.edu; username=p32003f; password=kiexeiH7veiqu9Uta6Go")) 
            {
                conn.Open();
            }
            if(username.Equals("a")) {
                ViewBag.invalid = true;
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}