
using System.Collections;
using System.Data;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ControllerDI.Interfaces;
using Npgsql;

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
			var cmd = QueryHandler.query(queryString);

			NpgsqlDataReader dr = cmd.ExecuteReader();

			ArrayList users = new ArrayList();
			while (dr.Read()) {
				users.Add(dr[0]);
			}
			dr.Close();
			ViewBag.users = users;
			
			queryString = "Select title From movies where title like '%" + search + "%'";
			cmd = QueryHandler.query(queryString);

			dr = cmd.ExecuteReader();

			ArrayList movies = new ArrayList();
			while (dr.Read()) {
				movies.Add(dr[0]);
			}
			dr.Close();
			ViewBag.movies = movies;
			
			return View();
		}
	}
}
