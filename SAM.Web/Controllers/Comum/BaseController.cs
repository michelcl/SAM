using System.Web;
using System.Web.Mvc;
using SAM.Web.Autenticacao;

using System;
using System.Collections.Generic;
using System.Linq;
using SAM.Negocio;
using SAM.Util;
using SAM.Web.Controllers.Comum;
using SAM.Web.ViewModels.Funcionario;

namespace SAM.Web.Controllers.Comum
{
    public class BaseController : Controller
    {
        protected virtual UsuarioAutenticado UsuarioLogado
        {
            get { return HttpContext.User as UsuarioAutenticado; }
        }

        public ActionResult MensagemSucesso()
        {
            return View("~/Views/Comum/MensagemSucesso.cshtml");
        }
    }
}
