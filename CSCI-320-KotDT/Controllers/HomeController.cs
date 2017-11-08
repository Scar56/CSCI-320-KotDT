
using System.Collections;
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
			ViewBag.username =
				System.Web.HttpContext.Current.Session["UserID"];
			return View();
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

            queryString = "SELECT distinct name FROM actors where lower(name) like lower('%" + search + "%') limit 25";
            ArrayList actors = QueryHandler.read(queryString, 1);
            ViewBag.actors = actors;

            ViewBag.search = search;

            return View();
		}
	}
}
