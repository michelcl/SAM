using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Newtonsoft.Json;
using SAM.Negocio;
using SAM.POCO;
using SAM.Util.Email;
using SAM.Web.Autenticacao;
using SAM.Web.Controllers.Comum;
using SAM.Web.ViewModels.Home;
/*
TODO: Criar error page
http://kitsula.com/Article/MVC-Custom-Error-Pages
TODO: Utilizar IPrincipal, MemberShip e Role
http://stackoverflow.com/questions/1064271/asp-net-mvc-set-custom-iidentity-or-iprincipal
http://www.mattwrock.com/post/2009/10/14/Implementing-custom-Membership-Provider-and-Role-Provider-for-Authinticating-ASPNET-MVC-Applications.aspx
             
http://stackoverflow.com/questions/2771094/how-do-i-create-a-custom-membership-provider-for-asp-net-mvc-2/2780406#2780406 
http://stackoverflow.com/questions/811137/how-to-implement-asp-net-membership-provider-in-my-domain-model?lq=1

 * 
 * ICONES
 * http://glyphicons.com/
 */



namespace SAM.Web.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Logado()
        {
            return View("HomeLogado");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return View("Index");
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                if (new CustomMembershipProvider().ValidateUser(loginViewModel.Login, loginViewModel.Senha))
                {
                    RegistrarUsuario(loginViewModel.Login);

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        Response.Redirect(returnUrl, true);
                        return null;
                    }
                    else
                        return Redirect("~/Home/Logado");
                }
                else
                {
                    ModelState.AddModelError("", "Login ou senha incorretos");
                    return View("Index");
                }
            }
            else
            {
                ModelState.AddModelError("", "Login ou senha incorretos");
                return View("Index");
            }
        }

        #region EsqueciMinhaSenha
        [HttpGet]
        public ActionResult EsqueciMinhaSenha()
        {
            return View("EsqueciMinhaSenha");
        }

        [HttpPost]
        public ActionResult EsqueciMinhaSenha(EsqueciMinhaSenhaViewModel esqueciMinhaSenhaViewModel)
        {
            if (ModelState.IsValid)
            {
                var usuario = new Negocio.Usuario();

                if (usuario.Listar(esqueciMinhaSenhaViewModel.Login) != null)
                {
                    var urlRedefinirSenha = "http://" + Request.Url.Authority + Url.Action("RedefinirSenha", "Home");
                    usuario.RedefinirSenha(esqueciMinhaSenhaViewModel.Login, urlRedefinirSenha);

                    Response.Redirect("EsqueciMinhaSenha?msg=ok");
                    return null;
                }
                else
                {
                    ModelState.AddModelError("", string.Format(@"Esse login {0} não existe. Tente outro login. 
                    Caso não se lembre qual é o seu login entre em contato com o administrador responsável da sua empresa.", esqueciMinhaSenhaViewModel.Login));
                    return View();
                }
            }
            return RedirectToAction("EsqueciMinhaSenha");
        }
        #endregion

        #region RedefinirSenha
        [HttpGet]
        public ActionResult RedefinirSenha(string l, string g, string msg)
        {
            Guid guid;

            if (Guid.TryParse(g, out guid))
            {
                var usuario = new Negocio.Usuario().ValidarRedefinirSenha(l, guid.ToString());

                if (usuario != null)
                {
                    var redefinirSenhaViewModel = new RedefinirSenhaViewModel()
                    {
                        Login = usuario.Login,
                        Guid = guid.ToString()
                    };
                    return View(redefinirSenhaViewModel);
                }
            }

            return View();//Apresentar mensagem de erro.
        }

        [HttpPost]
        public ActionResult RedefinirSenha(RedefinirSenhaViewModel redefinirSenhaViewModel)
        {
            if (ModelState.IsValid)
            {
                new Negocio.Usuario().AtualizarSenha(redefinirSenhaViewModel.Login, redefinirSenhaViewModel.Senha);

                return RedirectToAction("RedefinirSenha", "Home", new { msg = "ok" });
            }
            return View();
        }
        #endregion

        private void RegistrarUsuario(string login)
        {
            var usuario = new Negocio.Usuario().Listar(login);

            var serializeModel = new UsuarioAutenticadoSerializeModel
            {
                IdUsuario = usuario.IdUsuario,
                Login = usuario.Login,
                Nome = usuario.Nome,
                IdEmpresa = usuario.IdEmpresa
            };

            var userData = new JavaScriptSerializer().Serialize(serializeModel);
            var authTicket = new FormsAuthenticationTicket(1, login, DateTime.Now, DateTime.Now.AddHours(4), false, userData);

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(faCookie);
        }

        [HttpPost]
        public void AtualizarEmpresaLogada(int idEmpresa)
        {
            UsuarioLogado.AtualizarIdEmpresa(idEmpresa);

            Response.Redirect("~/Home/Logado");
        }
    }
}
