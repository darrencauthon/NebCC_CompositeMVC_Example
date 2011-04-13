using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcTurbine.Routing;

namespace MvcMusicStore.Routing
{
    public class DefaultRouting : IRouteRegistrator
    {
        public void Register(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Home", action = "Index", id = UrlParameter.Optional}, // Parameter defaults
                new {controller = new ControllersOnlyRouteConstraint()}
                );
        }
    }

    public class ControllersOnlyRouteConstraint : IRouteConstraint
    {
        private List<string> controllers = new List<string>();

        public ControllersOnlyRouteConstraint()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var controllerTypes = GetTheControllerTypesInThisAssembly(assembly);
                if (controllerTypes.Any())
                    controllers.AddRange(controllerTypes);
            }
        }

        private List<string> GetTheControllerTypesInThisAssembly(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(x => x.GetInterfaces().Contains(typeof (IController)))
                .Where(x=>x.IsAbstract == false && x.IsInterface == false)
                .Select(x=>x.Name.Replace("Controller", "")).ToList();
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var controller = values["controller"] as string;
            return (controllers.Contains(controller));
        }
    }
}