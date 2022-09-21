using Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace TalentManager
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var handler = new MyPremiumSecurityHandler(){
                InnerHandler = new MyOtherPremiumSecurityHandler(){
                    InnerHandler = new HttpControllerDispatcher(config)
                }
            };

            config.Routes.MapHttpRoute(
                name: "premiumApi",
                routeTemplate: "premium/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: null,
                handler: handler
            );

            config.Routes.MapHttpRoute(
               name: "DefaultApi",
               routeTemplate: "api/{controller}/{id}",
               defaults: new { id = RouteParameter.Optional }
           );

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Never;

            //config.Filters.Add(new AuthorizeAttribute());
            //config.MessageHandlers.Add(new MySecurityHandler());
            //config.MessageHandlers.Add(new MyHandler());
            //config.MessageHandlers.Add(new MethodOverrideHandler());
            config.MessageHandlers.Add(new CookiesHandler());

            // Quite los comentarios de la siguiente línea de código para habilitar la compatibilidad de consultas para las acciones con un tipo de valor devuelto IQueryable o IQueryable<T>.
            // Para evitar el procesamiento de consultas inesperadas o malintencionadas, use la configuración de validación en QueryableAttribute para validar las consultas entrantes.
            // Para obtener más información, visite http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // Para deshabilitar el seguimiento en la aplicación, incluya un comentario o quite la siguiente línea de código
            // Para obtener más información, consulte: http://www.asp.net/web-api
            //config.EnableSystemDiagnosticsTracing();
        }
    }
}
