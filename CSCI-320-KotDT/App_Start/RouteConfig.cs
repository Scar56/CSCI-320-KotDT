﻿using System.Web.Mvc;
using System.Web.Routing;

namespace CSCI_320_KotDT
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default",
				"{controller}/{action}/{id}",
				new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				"Movie",
				"{controller}/{action}/{id}",
				new { Controller = "Movie", action = "Search", id = UrlParameter.Optional }
			);
		}
	}
}
