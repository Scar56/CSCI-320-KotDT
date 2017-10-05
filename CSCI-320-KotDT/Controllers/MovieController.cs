using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControllerDI.Interfaces;
using Npgsql;

namespace CSCI_320_KotDT.Controllers
{
	public class MovieController : Controller
	{
		public MovieController(IQueryHandler IQuery) {
		}
		
		public ActionResult Display(String title = "alphabet")
		{
			string connstring = "Host=reddwarf.cs.rit.edu;" +
			   "Username=p32003f;Password=kiexeiH7veiqu9Uta6Go;Database=p32003f;";

			using (var conn = new NpgsqlConnection(connstring))
			{
				conn.Open();
				ViewBag.Message = "It's open";
				using (var cmd = new NpgsqlCommand())
				{
					cmd.Connection = conn;
					cmd.CommandText = "SELECT title FROM movies WHERE title like '%" + title + "%'";
					using (var reader = cmd.ExecuteReader())
					{
						string result = "";
						while (reader.Read())
						{
							result += reader.GetString(0) + "\n";

						}
						ViewBag.Message = result;
					}

				}
			}

			return View();
		}
	}
}
