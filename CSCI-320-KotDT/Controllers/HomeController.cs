﻿using System;
using System.Collections.Generic;
using System.Linq;
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
		
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult About() {
			ViewBag.Message = "Your application description page.";
			
			return View();
		}
		
		public ActionResult Contact() {
			ViewBag.Message = "Your contact page.";
			
			return View();
		}
	}
}
