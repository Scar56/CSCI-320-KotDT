using System;
using System.Collections;
using System.Collections.Generic;
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
                currUser = ((User) System.Web.HttpContext.Current.Session["selectedUser"]).username;
            
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
            queryString = "select * from review inner join movies on review.movie_id=movies.id where created_by = '" + username + "' order by date_create desc";
            ArrayList dr = QueryHandler.read(queryString, 9);
            List<Review> reviews = new List<Review>();
            foreach (ArrayList i in dr) {
                Review r = new Review((int) i[6]);
                r.MovieTitle = (string) i[8];
                r.Id = (int) i[4];
                r.CreatedBy = (string) i[5];
                r.DislikeCount = (int) i[3];
                r.LikeCount = (int) i[2];
                r.Score = (float) i[1];
                r.ReviewText = (string) i[0];
                reviews.Add(r);
            }
            ViewBag.feed = reviews;

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

        public ActionResult anonymize() {
            User user = (User)System.Web.HttpContext.Current.Session["UserID"];
            string query = "UPDATE \"User\" set is_anonymous = " + !user.isAnon + " WHERE username = '" + user.username + "'";
            QueryHandler.nonQuery(query);
            user.isAnon = !user.isAnon;
            return RedirectToAction("User", "User", new{username = user.username});
        }

        [HttpPost]
        public ActionResult update(string firstname, string lastname, string password) {
            User user = (User)System.Web.HttpContext.Current.Session["UserID"];
            string query = "UPDATE \"User\" set first_name = '" + firstname + "', last_name = '" + lastname + "', password = '" + password + "' WHERE username = '" + user.username + "'";
            QueryHandler.nonQuery(query);
            return RedirectToAction("User", "User", new{username = user.username});
        }
    }
}