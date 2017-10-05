using System;
using System.Web.Mvc;
using ControllerDI.Interfaces;

namespace CSCI_320_KotDT.Controllers
{
	public class MovieController : Controller
	{
		private readonly IQueryHandler QueryHandler;

		public MovieController(IQueryHandler IQuery) {
			QueryHandler = IQuery;
		}
		
		public ActionResult Display(String title = "alphabet")
		{
			
			ViewBag.Message = "It's open";
			string queryString = "SELECT title FROM movies WHERE title like '%" + title + "%'";
                                 					
			var cmd = QueryHandler.query(queryString);
				
			using (var reader = cmd.ExecuteReader())
			{
				string result = "";
				while (reader.Read())
				{
					result += reader.GetString(0) + "\n";

				}
				ViewBag.Message = result;
			}

				

			return View();
		}
	}
}
