using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using ControllerDI.Interfaces;

namespace CSCI_320_KotDT.Controllers
{
	public class HomeController : Controller
	{
		
		public HomeController(IQueryHandler IQuery) {
		}
		
		public ActionResult Index() {
			ViewBag.username =
				System.Web.HttpContext.Current.Session["UserID"];
			return View();
		}

		[HttpPost]
		public ActionResult Search(string search) {
			return View();
		}
	}
}
