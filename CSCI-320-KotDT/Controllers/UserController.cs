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
            string queryString = "SELECT username, first_name, last_name FROM \"User\" WHERE username = '" + username + "'";

            //should at most have one result
            ArrayList query = QueryHandler.read(queryString, 3);
            System.Web.HttpContext.Current.Session["selectedUser"] = new User(username, (string) ((ArrayList) query[0])[1], (string) ((ArrayList) query[0])[2]);
            ViewBag.followable = 0;
            
            string currUser = null;
            if(System.Web.HttpContext.Current.Session["UserID"] != null)
                currUser = ((User) System.Web.HttpContext.Current.Session["UserID"]).username;
            
            if (currUser != null) {
                ViewBag.followable = 1;
                queryString = "SELECT follower, following FROM follows_user WHERE follower = '" +
                              ((User) System.Web.HttpContext.Current.Session["UserID"]).username + "'";
    
                query = QueryHandler.read(queryString, 2);
                foreach (ArrayList i in query) {
                    if(currUser.Equals(i[1]))
                        ViewBag.followable = 2;
                }
                
            }
        

        return View();
        }

        //adds usernames to follows_user
        public ActionResult follow() {
            string currUser = ((User)System.Web.HttpContext.Current.Session["UserID"]).username;
            string selUser = ((User)System.Web.HttpContext.Current.Session["selectedUser"]).username;
            string query = "INSERT INTO follows_user " +
                               "(follower, following) " + 
                                    "VALUES('" + currUser + "', '" + selUser + "')";
            QueryHandler.nonQuery(query);
            return RedirectToAction("User", "User", new{username = selUser});
        }
        
        public ActionResult unfollow() {
            string currUser = ((User) System.Web.HttpContext.Current.Session["UserID"]).username;
            string selUser = ((User) System.Web.HttpContext.Current.Session["selectedUser"]).username;
            string query = "DELETE FROM follows_user " +
                           "WHERE (follower = '" + currUser + "' AND  following = '" + selUser + "')";
            QueryHandler.nonQuery(query);
            return RedirectToAction("User", "User", new{username = selUser});
        }
    }
}