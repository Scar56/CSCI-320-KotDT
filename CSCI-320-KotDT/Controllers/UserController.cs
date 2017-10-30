using System;
using System.Collections;
using System.Web.Mvc;
using ControllerDI.Interfaces;
using CSCI_320_KotDT.Models;

namespace CSCI_320_KotDT.Controllers {
    public class UserController : Controller{
        
        private readonly IQueryHandler QueryHandler;

        public UserController(IQueryHandler IQuery) {
            QueryHandler = IQuery;
        }

        [HttpGet]
        public ActionResult User(string username) {
            string queryString = "Select username, first_name, last_name From \"User\" where username = '" + username + "'";
            
            //should at most have one result
            ArrayList query = QueryHandler.read(queryString, 3);
            Console.WriteLine(username);
            System.Web.HttpContext.Current.Session["selectedUser"] = new User(username, (string) ((ArrayList)query[0])[1], (string) ((ArrayList)query[0])[2]);
            
            return View();
        }
    }
}