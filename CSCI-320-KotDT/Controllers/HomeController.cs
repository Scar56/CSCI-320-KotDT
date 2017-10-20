
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

			ArrayList res = new ArrayList();
			while (dr.Read()) {
				res.Add(dr[0]);
			}
			ViewBag.searchRes = res;
			
			
			return View();
		}
	}
}
