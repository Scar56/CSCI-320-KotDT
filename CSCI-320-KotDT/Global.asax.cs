using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using ControllerDI.Interfaces;
using ControllerDI.Services;
using CSCI_320_KotDT.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace CSCI_320_KotDT
{
	public class Global : HttpApplication
	{
		protected void Application_Start()
		{
			ControllerBuilder.Current.SetControllerFactory(new controllerFactory());
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}
		
		public void ConfigureServices(IServiceCollection services)
		{
			// Add application services.
			services.AddSingleton<IQueryHandler, QueryHandler>();
		}
	}
}
