using System;
using System.Web.Mvc;
using ControllerDI.Interfaces;

namespace CSCI_320_KotDT.Controllers {
    public class LoginController : Controller {
        private readonly IQueryHandler QueryHandler;

        public LoginController(IQueryHandler IQuery) {
            QueryHandler = IQuery;
        }
        
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
            
            string queryString = "Select * From \"User\" where username = '" + username + "'";
            var cmd = QueryHandler.query(queryString);
            var dr = cmd.ExecuteReader();
            while(dr.Read())
                Console.WriteLine(dr[0]);
            //check validity
            if(username.Equals("a")) {
                ViewBag.invalid = true;
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}