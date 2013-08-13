using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace SAM.Web.Autenticacao
{
    public class UsuarioAutenticado : IUsuarioAutenticado, IPrincipal
    {
        public int IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public int IdEmpresa { get; set; }
        public IIdentity Identity { get; private set; }

        public UsuarioAutenticado(string login)
        {
            this.Identity = new GenericIdentity(login);
        }

        protected virtual UsuarioAutenticado UsuarioLogado
        {
            get { return HttpContext.Current.User as UsuarioAutenticado; }
        }

        public void AtualizarIdEmpresa(int idEmpresa)
        {
            var serializeModel = new UsuarioAutenticadoSerializeModel
            {
                IdUsuario = UsuarioLogado.IdUsuario,
                Login = UsuarioLogado.Login,
                Nome = UsuarioLogado.Nome,
                IdEmpresa = idEmpresa
            };

            var userData = new JavaScriptSerializer().Serialize(serializeModel);
            var authTicket = new FormsAuthenticationTicket(1, UsuarioLogado.Login, DateTime.Now, DateTime.Now.AddHours(4), false, userData);

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);

            HttpContext.Current.Response.Cookies.Add(faCookie);
        }
           

        public bool IsInRole(string role)
        {
            var papeis = new Negocio.Papel().Listar(Login);

            foreach (var item in papeis)
                if (item.Descricao == role)
                    return true;

            return false;
            ///if (Role.Description.ToLower() == role.ToLower())
                //return true;

 //           return false;
        }
    }
}