using System;
using System.Web.Mvc;
using System.Web.SessionState;
using ControllerDI.Interfaces;
using ControllerDI.Services;

namespace CSCI_320_KotDT.Controllers {
    public class controllerFactory : DefaultControllerFactory
    { 
//        private readonly string _controllerNamespace;
//        
//        public controllerFactory(string controllerNamespace)
//        {
//            _controllerNamespace = controllerNamespace;
//        }
//        
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            IQueryHandler iHandler = new QueryHandler();
            IController controller = Activator.CreateInstance(controllerType, new[] { iHandler }) as Controller;
            return controller;
//            if (requestContext == null)
//                throw new ArgumentNullException("requestContext");
//            if (string.IsNullOrEmpty(controllerName) && !requestContext.RouteData.HasDirectRouteMatch())
//                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "controllerName");
//            Type controllerType = GetControllerType(requestContext, controllerName);
//            return GetControllerInstance(requestContext, controllerType);
            
        }
        public System.Web.SessionState.SessionStateBehavior GetControllerSessionBehavior(
            System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            return SessionStateBehavior.Default;
        }
        public void ReleaseController(IController controller)
        {
            IDisposable disposable = controller as IDisposable;
            if (disposable != null)
                disposable.Dispose();
        }
    } 
}