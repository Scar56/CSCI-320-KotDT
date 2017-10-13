using System;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Services;
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
        
        [HttpGet]
        public ActionResult Logout() {
            
            System.Web.HttpContext.Current.Session["UserID"] = null;
            return RedirectToAction("Index", "Home");
        }
        [HttpPost][WebMethod(EnableSession = true)]
        public ActionResult Login(string username, string password) {
            string queryString = "Select username, password From \"User\" where username = '" + username + "'";
            var cmd = QueryHandler.query(queryString);
            var dr = cmd.ExecuteReader();
            //should at most have one result
            while(dr.Read()){
                if (password.Equals(dr[1])) {
                    dr.Close();
                    System.Web.HttpContext.Current.Session["UserID"] = username;
                    return RedirectToAction("Index", "Home");
                }
                dr.Close();
            }
            return View();
        }
    
        
        [HttpGet]
        public ActionResult CreateUser() {
            ViewBag.invalid = false;
            return View();
        }
        
        [HttpPost]
        public ActionResult CreateUser(string username, string password, string firstName, string lastName) {
            if(username.Equals("")){
                ViewBag.invalid = true;
                ViewBag.invalidMessage = "A username is required";
                return View();
            }
            
            if(password.Equals("")){
                ViewBag.invalid = true;
                ViewBag.invalidMessage = "A password is required";
                return View();
            }
            
            if(firstName.Equals("")){
                ViewBag.invalid = true;
                ViewBag.invalidMessage = "Please enter a first name";
                return View();
            }
            
            if(lastName.Equals("")){
                ViewBag.invalid = true;
                ViewBag.invalidMessage = "Please enter a last name";
                return View();
            }
            
            string queryString = "Select * From \"User\" where username = '" + username + "'";
            var cmd = QueryHandler.query(queryString);
            var dr = cmd.ExecuteReader();
            while(dr.Read()){
                //check validity
                if(username.Equals(dr[0])) {
                    ViewBag.invalid = true;
                    ViewBag.invalidMessage = "username invalid";
                    return View();
                }
            }
            dr.Close();
            
            ViewBag.invalid = false;
            queryString = "INSERT INTO \"User\" VALUES('" + username + "'" + ", " + "'" + password + "'" + ", " + "'" + firstName + "'" + ", " + "'" + lastName + "'" + ", " + false + ")";
            cmd.CommandText = queryString;
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index", "Home");
        }
    }
}