using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RESTapiPruebaTeen
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            // Rutas para los endpoints de chistes
            config.Routes.MapHttpRoute(
                name: "ChistesApi",
                routeTemplate: "api/chistes/{tipo}",
                defaults: new { controller = "Chistes", tipo = RouteParameter.Optional }
            );

            // Rutas para los endpoints matemáticos
            config.Routes.MapHttpRoute(
                name: "MatematicoApi",
                routeTemplate: "api/matemático/{action}",
                defaults: new { controller = "Matematico" }
            );

        }
    }
}
