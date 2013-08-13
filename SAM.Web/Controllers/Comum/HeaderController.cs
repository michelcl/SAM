using System.Collections.Generic;
using System.Web.Mvc;
using SAM.Web.ViewModels.Comum;

namespace SAM.Web.Controllers.Comum
{
    public class HeaderController : BaseController
    {
        [ChildActionOnly]
        public ActionResult HeaderUsuarioLogado()
        {
            var funcionario = new Negocio.Funcionario().Listar(UsuarioLogado.Login);
            IEnumerable<POCO.Empresa> listaEmpresa = null;

            if (UsuarioLogado.IsInRole("Administrador do sistema"))
                listaEmpresa = new Negocio.Empresa().Listar();

            if (funcionario != null)
                return View("~/Views/Comum/UsuarioLogado.cshtml", new UsuarioLogadoViewModel { Nome = UsuarioLogado.Nome, Login = UsuarioLogado.Login, Foto = funcionario.Foto, IdEmpresa = UsuarioLogado.IdEmpresa, ListaEmpresa = listaEmpresa });
            else
                return View("~/Views/Comum/UsuarioLogado.cshtml", new UsuarioLogadoViewModel { Nome = UsuarioLogado.Nome, Login = UsuarioLogado.Login, IdEmpresa = UsuarioLogado.IdEmpresa, ListaEmpresa = listaEmpresa });
        }
    }
}
