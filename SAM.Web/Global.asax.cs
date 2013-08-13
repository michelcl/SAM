using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using SAM.Web.Autenticacao;

namespace SAM.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var serializer = new JavaScriptSerializer();
                var serializeModel = serializer.Deserialize<UsuarioAutenticadoSerializeModel>(authTicket.UserData);
                if (serializeModel != null)
                {
                    var newUser = new UsuarioAutenticado(authTicket.Name)
                        {
                            IdUsuario = serializeModel.IdUsuario,
                            Login = serializeModel.Login,
                            Nome = serializeModel.Nome,
                            IdEmpresa = serializeModel.IdEmpresa,
                        };

                    HttpContext.Current.User = newUser;
                }
            }
        }
    }
}