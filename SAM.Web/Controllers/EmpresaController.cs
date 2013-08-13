using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAM.Negocio;
using SAM.Web.Controllers.Comum;
using SAM.Util;
using SAM.Web.ViewModels.Empresa;
using SAM.Web.ViewModels.Filial;

namespace SAM.Web.Controllers
{
    [Authorize]
    public class EmpresaController : BaseController
    {
        public ActionResult Index()
        {
            return View("Listar");
        }

        #region TELA LISTAGEM EMPRESAS
        [HttpPost]
        public JsonResult Listar(int jtStartIndex, int jtPageSize, string jtSorting = null, string pesquisa = "")
        {
            pesquisa = pesquisa.Trim();

            try
            {
                IEnumerable<POCO.Empresa> empresa;

                if (UsuarioLogado.IsInRole("Administrador do sistema")) //Lista todas as empresas.
                    empresa = new Empresa().Listar()
                    .Where(p => pesquisa == ""
                        || p.Cnpj.ContainsIgnoraAcento(pesquisa.Replace(".", "").Replace("/", "").Replace("-", ""))
                        || p.NomeFantasia.ContainsIgnoraAcento(pesquisa)
                        || p.RazaoSocial.ContainsIgnoraAcento(pesquisa)
                        || p.Segmento.ContainsIgnoraAcento(pesquisa)
                          )
                    .OrderBy(p => p.NomeFantasia);
                else
                    empresa = new Empresa().Listar(UsuarioLogado.IdEmpresa)
                    .Where(p => pesquisa == ""
                        || p.Cnpj.ContainsIgnoraAcento(pesquisa.Replace(".", "").Replace("/", "").Replace("-", ""))
                        || p.NomeFantasia.ContainsIgnoraAcento(pesquisa)
                        || p.RazaoSocial.ContainsIgnoraAcento(pesquisa)
                        || p.Segmento.ContainsIgnoraAcento(pesquisa)
                          )
                    .OrderBy(p => p.NomeFantasia);

                var empresaListarViewModel = new List<EmpresaListarViewModel>();

                foreach (var item in empresa.Skip(jtStartIndex - 1).Take(jtPageSize))
                {
                    empresaListarViewModel.Add(new EmpresaListarViewModel
                    {
                        IdEmpresa = item.IdEmpresa,
                        Cnpj = item.Cnpj.FormatarCNPJ(),
                        RazaoSocial = item.RazaoSocial,
                        NomeFantasia = item.NomeFantasia,
                        Segmento = item.Segmento,
                        Ativo = item.Ativo ? "Ativo" : "Inativo",
                    });
                }

                return Json(new
                {
                    Result = "OK",
                    Records = empresaListarViewModel,
                    TotalRecordCount = empresa.Count()
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = ex.Message
                });
            }
        }

        #endregion

        #region TELA VISUALIZAR
        [HttpGet]
        public ActionResult Visualizar(int id = 0)
        {
            EmpresaVisualizarViewModel empresaVisualizarViewModel;

            if (id != 0)
            {
                var empresa = new Negocio.Empresa().Listar(id).FirstOrDefault();

                AtualizarIdEmpresaUsuarioLogado(empresa.IdEmpresa);

                empresaVisualizarViewModel = new EmpresaVisualizarViewModel()
                {
                    IdEmpresa = empresa.IdEmpresa,
                    NomeFantasia = empresa.NomeFantasia,
                    RazaoSocial = empresa.RazaoSocial,
                    Dominio = empresa.Dominio,
                    DominioInicial = empresa.Dominio,
                    Cnpj = empresa.Cnpj.FormatarCNPJ(),
                    Ativo = empresa.Ativo,
                    Segmento = empresa.Segmento,
                    Telefone1 = empresa.Telefone1,
                    Telefone2 = empresa.Telefone2,
                    Site = empresa.Site,
                };
            }
            else
            {
                empresaVisualizarViewModel = new EmpresaVisualizarViewModel();
            }

            return View(empresaVisualizarViewModel);
        }
        #endregion

        #region TELA CADASTRAR

        [HttpGet]
        [Authorize(Roles = "Administrador do sistema, Administrador da empresa")]
        public ActionResult Cadastrar(int id = 0)
        {
            EmpresaCadastrarViewModel empresaCadastrarViewModel;

            if (id != 0)
            {
                var empresa = new Negocio.Empresa().Listar(id).FirstOrDefault();

                AtualizarIdEmpresaUsuarioLogado(empresa.IdEmpresa);

                empresaCadastrarViewModel = new EmpresaCadastrarViewModel()
                {
                    IdEmpresa = empresa.IdEmpresa,
                    NomeFantasia = empresa.NomeFantasia,
                    RazaoSocial = empresa.RazaoSocial,
                    Dominio = empresa.Dominio,
                    DominioInicial = empresa.Dominio,
                    Cnpj = empresa.Cnpj.FormatarCNPJ(),
                    Ativo = empresa.Ativo,
                    Segmento = empresa.Segmento,
                    Telefone1 = empresa.Telefone1,
                    Telefone2 = empresa.Telefone2,
                    Site = empresa.Site,
                };
            }
            else
            {
                empresaCadastrarViewModel = new EmpresaCadastrarViewModel();
            }

            return View(empresaCadastrarViewModel);
        }
        [HttpPost]
        public ActionResult Cadastrar(EmpresaCadastrarViewModel empresaCadastrarViewModel)
        {

            var empresa = new POCO.Empresa()
            {
                IdEmpresa = empresaCadastrarViewModel.IdEmpresa,
                NomeFantasia = empresaCadastrarViewModel.NomeFantasia,
                RazaoSocial = empresaCadastrarViewModel.RazaoSocial,
                Logo = "",
                Cnpj = empresaCadastrarViewModel.Cnpj,
                Segmento = empresaCadastrarViewModel.Segmento,
                Site = empresaCadastrarViewModel.Site,
                Telefone1 = empresaCadastrarViewModel.Telefone1,
                Telefone2 = empresaCadastrarViewModel.Telefone2,
                Ativo = empresaCadastrarViewModel.Ativo,
                Dominio = empresaCadastrarViewModel.Dominio,
            };

            var idEmpresa = new Empresa().Salvar(empresa);

            if (Request["acao"] == "Salvar e Novo")
                return RedirectToAction("Cadastrar", new { id = 0 });
            else
                return RedirectToAction("Visualizar", new { id = idEmpresa });
        }

        #region validacoes
        [HttpPost]
        public JsonResult VerificarSeDominioExiste(string dominio, string dominioInicial)
        {
            if (dominio != dominioInicial)
                return Json(!new Negocio.Empresa().VerificarSeDominioExiste(dominio));

            return Json(true);//Dominio não foi alterado
        }
        #endregion
        #endregion

        private void AtualizarIdEmpresaUsuarioLogado(int idEmpresa)
        {
            //Seta a empresa que o usuário está querendo ADM quer utillizar.
            if (UsuarioLogado.IsInRole("Administrador do sistema"))
                if (UsuarioLogado.IdEmpresa != idEmpresa)
                {
                    UsuarioLogado.AtualizarIdEmpresa(idEmpresa);
                    Response.Redirect(Request.Url.ToString(), true);
                }
        }
    }
}
