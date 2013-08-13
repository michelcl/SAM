using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAM.Web.ViewModels.Filial;
using SAM.Util;
using SAM.Negocio;
using SAM.Web.Controllers.Comum;

namespace SAM.Web.Controllers
{
    public class FilialController : BaseController
    {
        //
        // GET: /Filial/

        public ActionResult Index()
        {
            return View();
        }

        #region TELA LISTAGEM FILIAIS
        [HttpPost]
        public JsonResult Listar(int idEmpresa)
        {
            try
            {
                var filial = new Filial().Listar(idEmpresa).OrderBy(p => p.NomeFantasia);

                var filialListarViewModel = new List<FilialListarViewModel>();

                foreach (var item in filial)
                {
                    filialListarViewModel.Add(new FilialListarViewModel
                    {
                        IdFilial = item.IdFilial,
                        Cnpj = item.Cnpj.FormatarCNPJ(),
                        RazaoSocial = item.RazaoSocial,
                        NomeFantasia = item.NomeFantasia,
                        NomeEmpresa = item.Empresa.NomeFantasia,
                        Ativo = item.Ativo ? "Ativo" : "Inativo",
                    });
                }

                return Json(new
                {
                    Result = "OK",
                    Records = filialListarViewModel,
                    TotalRecordCount = filial.Count()
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

        #region TELA CADASTRAR
        [HttpGet]
        [Authorize(Roles = "Administrador do sistema, Administrador da empresa")]
        public ActionResult Cadastrar(int id = 0, int? idEmpresa = null)
        {
            FilialCadastrarViewModel filialCadastrarViewModel;

            if (id != 0)
            {
                var filial = new Negocio.Filial().Listar(null, id).FirstOrDefault();
                var empresa = new Negocio.Empresa().Listar(filial.IdEmpresa).FirstOrDefault();

                AtualizarIdEmpresaUsuarioLogado(empresa.IdEmpresa);

                filialCadastrarViewModel = new FilialCadastrarViewModel()
                {
                    IdFilial = filial.IdFilial,
                    NomeFantasia = filial.NomeFantasia,
                    RazaoSocial = filial.RazaoSocial,
                    Cnpj = filial.Cnpj.FormatarCNPJ(),
                    Telefone1 = filial.Telefone1,
                    Telefone2 = filial.Telefone2,
                    Ativo = filial.Ativo,

                    IdEmpresa = filial.IdEmpresa,
                    NomeFantasiaEmpresa = empresa.NomeFantasia,
                };
            }
            else
            {
                var empresa = new Negocio.Empresa().Listar(idEmpresa.Value).FirstOrDefault();
                filialCadastrarViewModel = new FilialCadastrarViewModel()
                {
                    IdEmpresa = idEmpresa.Value,
                    NomeFantasiaEmpresa = empresa.NomeFantasia,
                };
            }

            return View("Cadastrar", filialCadastrarViewModel);
        }

        [HttpPost]
        public ActionResult Cadastrar(FilialCadastrarViewModel filialCadastrarViewModel)
        {
            var filial = new POCO.Filial()
            {
                Ativo = filialCadastrarViewModel.Ativo,
                Cnpj = filialCadastrarViewModel.Cnpj,
                IdEmpresa = filialCadastrarViewModel.IdEmpresa,
                IdFilial = filialCadastrarViewModel.IdFilial,
                NomeFantasia = filialCadastrarViewModel.NomeFantasia,
                RazaoSocial = filialCadastrarViewModel.RazaoSocial,
                Telefone1 = filialCadastrarViewModel.Telefone1,
                Telefone2 = filialCadastrarViewModel.Telefone2,
            };

            var idFilial = new Filial().Salvar(filial);

            if (Request["acao"] == "Salvar e Novo")
                return RedirectToAction("Cadastrar", new { idEmpresa = filialCadastrarViewModel.IdEmpresa });
            else
                return RedirectToAction("Visualizar", new { id = idFilial });
        }
        #endregion
        #region TELA VISUALIZAR
        [HttpGet]
        public ActionResult Visualizar(int id = 0)
        {
            FilialVisualizarViewModel filialVisualizarViewModel;

            if (id != 0)
            {
                var filial = new Negocio.Filial().Listar(null, id).FirstOrDefault();
                var empresa = new Negocio.Empresa().Listar(filial.IdEmpresa).FirstOrDefault();
                AtualizarIdEmpresaUsuarioLogado(empresa.IdEmpresa);
                filialVisualizarViewModel = new FilialVisualizarViewModel()
                {
                    IdFilial = filial.IdFilial,
                    NomeFantasia = filial.NomeFantasia,
                    RazaoSocial = filial.RazaoSocial,
                    Cnpj = filial.Cnpj.FormatarCNPJ(),
                    Telefone1 = filial.Telefone1,
                    Telefone2 = filial.Telefone2,
                    Ativo = filial.Ativo,

                    IdEmpresa = filial.IdEmpresa,
                    NomeFantasiaEmpresa = empresa.NomeFantasia,
                };
            }
            else
            {
                filialVisualizarViewModel = new FilialVisualizarViewModel();
            }

            return View(filialVisualizarViewModel);
        }
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
