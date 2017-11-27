
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using ControllerDI.Interfaces;
using CSCI_320_KotDT.Models;

namespace CSCI_320_KotDT.Controllers
{
	public class HomeController : Controller
	{
		
		private readonly IQueryHandler QueryHandler;

		public HomeController(IQueryHandler IQuery) {
			QueryHandler = IQuery;
		}
		
		public ActionResult Index() {
			User user = (User)System.Web.HttpContext.Current.Session["UserID"];
			string queryString;
			if(user==null){
				queryString = "select * from review";
				ArrayList dr = QueryHandler.read(queryString, 7);
				List<Review> reviews = new List<Review>();
				foreach(ArrayList i in dr) {
					Review r = new Review((int)i[6]);
					r.Id = (int) i[4];
					r.CreatedBy = (string)i[5];
					r.DislikeCount = (int)i[3];
					r.LikeCount = (int)i[2];
					r.Score = (float)i[1];
					r.ReviewText = (string)i[0];
					reviews.Add(r);
				}
				ViewBag.feed = reviews;
				return View();
				
			}else{
				queryString = "select * from follows_user  inner join review on follows_user.following=review.created_by where follower='" + user.username + "'";
				ArrayList dr = QueryHandler.read(queryString, 9);
				List<Review> reviews = new List<Review>();
				foreach(ArrayList i in dr) {
					Review r = new Review((int)i[8]);
					r.Id = (int) i[6];
					r.CreatedBy = (string)i[7];
					r.DislikeCount = (int)i[5];
					r.LikeCount = (int)i[4];
					r.Score = (float)i[3];
					r.ReviewText = (string)i[2];
					reviews.Add(r);
				}
				ViewBag.feed = reviews;
				return View();
			}
		}

		[HttpPost]
		public ActionResult Search(string search) {
			string queryString = "Select Username From \"User\" where username like '%" + search + "%'";

			ArrayList users = QueryHandler.read(queryString, 1);
			ViewBag.users = users;

            queryString = "SELECT title, id FROM movies where lower(title) like lower('%" + search + "%') " + Movie.OrderingString() + "limit 25";
            ArrayList dr = QueryHandler.read(queryString, 2);


            ArrayList movies = new ArrayList();
			ArrayList id = new ArrayList();
			foreach(ArrayList i in dr){
				movies.Add(i[0]);
				id.Add(i[1]);
			}
			ViewBag.movies = movies;
			ViewBag.id = id;

            queryString = "SELECT distinct name FROM actors where lower(name) like lower('%" + search + "%') union SELECT distinct name FROM directors where lower(name) like lower('%" + search + "%') limit 25";
            ArrayList actors = QueryHandler.read(queryString, 1);
            ViewBag.actors = actors;

            ViewBag.search = search;

            return View();
		}
		
		public ActionResult Like(int? id) {
			string queryString = "UPDATE  review set like_count = like_count+1 where review_id = " + id;
			Console.WriteLine(id);
			QueryHandler.nonQuery(queryString);
			return RedirectToAction("Index","Home");
		}
		
		public ActionResult Dislike(int? id) {
            string queryString = "UPDATE  review set dislike_count = dislike_count+1 where review_id = " + id;
            QueryHandler.nonQuery(queryString);
			return RedirectToAction("Index","Home");
		}
	}
}
