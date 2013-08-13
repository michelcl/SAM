using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SAM.Negocio;
using SAM.Util;
using SAM.Web.Controllers.Comum;
using SAM.Web.ViewModels.Funcionario;

namespace SAM.Web.Controllers
{
    [Authorize]
    public class FuncionarioController : BaseController
    {
        public ActionResult Index(int? id = null)
        {
            if (!id.HasValue)
                return View("Listar");
            else
                return RedirectToAction("Cadastrar", new { id = id.Value });
        }


        [HttpGet]
        public ActionResult PopupListar()
        {
            return View("PartialPopupListar");
        }

        #region TELA LISTAGEM FUNCIONARIOS
        [HttpPost]
        public JsonResult Listar(int jtStartIndex = 0, int jtPageSize = 10, string jtSorting = null, string pesquisa = "")
        {
            pesquisa = pesquisa.Trim();

            try
            {
                var funcionario = new Funcionario().Listar(UsuarioLogado.IdEmpresa)
                    .Where(p => pesquisa == ""
                        || p.Usuario.Nome.ContainsIgnoraAcento(pesquisa)
                        || p.Cargo != null && p.Cargo.Descricao.ContainsIgnoraAcento(pesquisa)
                        || p.Departamento != null && p.Departamento.Descricao.ContainsIgnoraAcento(pesquisa)
                          )
                    .OrderBy(p => p.Usuario.Nome);

                var funcionarioListaViewModel = new List<FuncionarioListaViewModel>();

                foreach (var item in funcionario.Skip(jtStartIndex - 1).Take(jtPageSize))
                {
                    funcionarioListaViewModel.Add(new FuncionarioListaViewModel
                    {
                        IdFuncionario = item.IdFuncionario,
                        Nome = item.Usuario.Nome,
                        Cargo = item.Cargo != null ? item.Cargo.Descricao : "",
                        Departamento = item.Departamento != null ? item.Departamento.Descricao : "",
                        Ativo = item.Usuario.Ativo ? "Ativo" : "Inativo"
                    });
                }

                return Json(new
                {
                    Result = "OK",
                    Records = funcionarioListaViewModel,
                    TotalRecordCount = funcionario.Count()
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

        #region TELA CADASTRO FUNCIONARIO

        [HttpGet]
        [Authorize(Roles = "Administrador do sistema, Administrador da empresa")]
        public ActionResult Cadastrar(int id = 0)
        {
            FuncionarioCadastrarViewModel funcionarioCadastrarViewModel;

            var empresa = new Empresa().Listar(UsuarioLogado.IdEmpresa).FirstOrDefault();
            var listaCargo = new Negocio.Cargo().Listar(UsuarioLogado.IdEmpresa);
            var listaDepartamento = new Negocio.Departamento().Listar(UsuarioLogado.IdEmpresa);
            var listaPapel = new Negocio.Papel().Listar();
            var listaSexo = new Negocio.Sexo().Listar();

            if (id != 0)
            {
                var funcionario = new Funcionario().Listar(null, id).FirstOrDefault();
                if (funcionario != null)
                {
                    funcionarioCadastrarViewModel = new FuncionarioCadastrarViewModel
                    {
                        IdFuncionario = funcionario.IdFuncionario,

                        Nome = funcionario.Usuario.Nome,
                        Login = funcionario.Usuario.Login.Substring(0, funcionario.Usuario.Login.IndexOf("@" + empresa.Dominio)),
                        LoginInicial = funcionario.Usuario.Login.Substring(0, funcionario.Usuario.Login.IndexOf("@" + empresa.Dominio)),
                        Email = funcionario.Usuario.Email,
                        Ativo = funcionario.Usuario.Ativo,
                        Telefone = funcionario.Telefone,
                        Celular = funcionario.Celular,
                        Foto = funcionario.Foto,

                        Matricula = funcionario.Matricula,
                        Sexo = funcionario.Sexo,
                        Skype = funcionario.Skype,
                        DataNascimento = funcionario.DataNascimento,
                        DataFuncao = funcionario.DataFuncao,

                        NomeEmpresa = empresa.NomeFantasia,
                        DominioEmpresa = empresa.Dominio,

                        IdUsuario = funcionario.IdUsuario,
                        IdCargo = funcionario.IdCargo,
                        IdDepartamento = funcionario.IdDepartamento,
                        IdPapel = funcionario.Usuario.IdPapel,

                        IdGestor = (funcionario.Gestor != null) ? funcionario.Gestor.IdFuncionario : (int?)null,
                        NomeGestor = (funcionario.Gestor != null) ? funcionario.Gestor.Usuario.Nome : string.Empty,

                        ListaCargo = listaCargo,
                        ListaDepartamento = listaDepartamento,
                        ListaPapel = listaPapel,
                        ListaSexo = listaSexo,
                    };
                }
                else
                {
                    throw new Exception("Funcionário não encontrado!");
                }
            }
            else
                funcionarioCadastrarViewModel = new FuncionarioCadastrarViewModel
                {
                    IdFuncionario = 0,
                    IdUsuario = 0,
                    NomeEmpresa = empresa.NomeFantasia,
                    DominioEmpresa = empresa.Dominio,
                    ListaCargo = listaCargo,
                    ListaDepartamento = listaDepartamento,
                    ListaPapel = listaPapel,
                    ListaSexo = listaSexo,
                };

            return View("Cadastrar", funcionarioCadastrarViewModel);
        }

        [HttpPost]
        public ActionResult Cadastrar(FuncionarioCadastrarViewModel funcionarioCadastrarViewModel)
        {
            if (ValidarFormulario(funcionarioCadastrarViewModel))
            {
                var usuario = new POCO.Usuario
                {
                    IdUsuario = funcionarioCadastrarViewModel.IdUsuario,
                    Nome = funcionarioCadastrarViewModel.Nome,
                    Login = funcionarioCadastrarViewModel.Login,
                    Senha = funcionarioCadastrarViewModel.Senha,
                    IdEmpresa = UsuarioLogado.IdEmpresa,
                    Ativo = funcionarioCadastrarViewModel.Ativo,
                    Email = funcionarioCadastrarViewModel.Email,
                    IdPapel = funcionarioCadastrarViewModel.IdPapel,
                };

                var funcionario = new POCO.Funcionario
                {
                    IdFuncionario = funcionarioCadastrarViewModel.IdFuncionario,
                    IdUsuario = funcionarioCadastrarViewModel.IdUsuario,
                    Telefone = funcionarioCadastrarViewModel.Telefone,
                    Celular = funcionarioCadastrarViewModel.Celular,
                    Foto = funcionarioCadastrarViewModel.Foto,
                    Matricula = funcionarioCadastrarViewModel.Matricula,
                    Sexo = funcionarioCadastrarViewModel.Sexo,
                    Skype = funcionarioCadastrarViewModel.Skype,
                    DataNascimento = funcionarioCadastrarViewModel.DataNascimento,
                    DataFuncao = funcionarioCadastrarViewModel.DataFuncao,

                    IdCargo = funcionarioCadastrarViewModel.IdCargo,
                    IdDepartamento = funcionarioCadastrarViewModel.IdDepartamento,
                    IdGestor = funcionarioCadastrarViewModel.IdGestor,
                    Usuario = usuario,
                };

                int idFuncionario = new Negocio.Funcionario().Salvar(funcionario);

                if (Request["acao"] == "Salvar e Novo")
                    return RedirectToAction("Cadastrar", new { id = 0 });
                else
                    return RedirectToAction("Visualizar", new { id = idFuncionario });
            }
            else
            {
                //funcionarioCadastroViewModel.IdFuncionario = 0;
                funcionarioCadastrarViewModel.ListaCargo = new Cargo().Listar(UsuarioLogado.IdEmpresa);
                funcionarioCadastrarViewModel.ListaDepartamento = new Departamento().Listar(UsuarioLogado.IdEmpresa);
                funcionarioCadastrarViewModel.ListaPapel = new Papel().Listar();
                funcionarioCadastrarViewModel.ListaSexo = new Negocio.Sexo().Listar();
                funcionarioCadastrarViewModel.NomeEmpresa = new Empresa().Listar(UsuarioLogado.IdEmpresa).FirstOrDefault().NomeFantasia;

                return View("Cadastrar", funcionarioCadastrarViewModel);
            }
        }

        #region validacoes
        [HttpPost]
        public JsonResult VerificarSeLoginExiste(string login, string loginInicial)
        {
            if (login != loginInicial)
                return Json(!new Negocio.Usuario().VerificarSeLoginExiste(login, UsuarioLogado.IdEmpresa));

            return Json(true);//Login não foi alterado
        }

        private bool ValidarFormulario(FuncionarioCadastrarViewModel funcionario)
        {
            if (funcionario.Login != funcionario.LoginInicial
                && new Negocio.Usuario().VerificarSeLoginExiste(funcionario.Login, UsuarioLogado.IdEmpresa))
                ModelState.AddModelError("Login", "Login já está sendo utilizado. Por favor, digite outro login.");
            return ModelState.IsValid;
        }
        #endregion
        #endregion

        #region Visualizar

        [HttpGet]
        public ActionResult Visualizar(int id = 0)
        {
            var empresa = new Empresa().Listar(UsuarioLogado.IdEmpresa).FirstOrDefault();
            //var listaCargo = new Negocio.Cargo().Listar(Usuario.IdEmpresa);
            //var listaDepartamento = new Negocio.Departamento().Listar(Usuario.IdEmpresa);
            //var listaPapel = new Negocio.Papel().Listar();
            //var listaSexo = new Negocio.Sexo().Listar();

            if (id != 0)
            {
                var funcionario = new Funcionario().Listar(null, id).FirstOrDefault();
                if (funcionario != null)
                {
                    var cargo = string.Empty;
                    var departamento = string.Empty;
                    var sexo = string.Empty;
                    var papel = string.Empty;

                    if (funcionario.IdCargo.HasValue)
                        cargo = new Negocio.Cargo().Listar(null, funcionario.IdCargo).FirstOrDefault().Descricao;
                    if (funcionario.IdDepartamento.HasValue)
                        departamento = new Negocio.Departamento().Listar(null, funcionario.IdDepartamento).FirstOrDefault().Descricao;

                    if (funcionario.Sexo == "M")
                        sexo = "Masculino";
                    else if (funcionario.Sexo == "F")
                        sexo = "Feminino";

                    var funcionarioCadastrarViewModel = new FuncionarioVisualizarViewModel
                    {
                        IdFuncionario = funcionario.IdFuncionario,

                        Nome = funcionario.Usuario.Nome,
                        Login = funcionario.Usuario.Login.Substring(0, funcionario.Usuario.Login.IndexOf("@" + empresa.Dominio)),
                        LoginInicial = funcionario.Usuario.Login.Substring(0, funcionario.Usuario.Login.IndexOf("@" + empresa.Dominio)),
                        Email = funcionario.Usuario.Email,
                        Ativo = funcionario.Usuario.Ativo,
                        Telefone = funcionario.Telefone,
                        Celular = funcionario.Celular,
                        Foto = funcionario.Foto,

                        Matricula = funcionario.Matricula,

                        Skype = funcionario.Skype,
                        DataNascimento = funcionario.DataNascimento,
                        Idade = CalcularIdade(funcionario.DataNascimento),
                        DataFuncao = funcionario.DataFuncao,
                        TempoFuncao = CalcularTempoFuncao(funcionario.DataFuncao),

                        NomeEmpresa = empresa.NomeFantasia,
                        DominioEmpresa = empresa.Dominio,

                        IdUsuario = funcionario.IdUsuario,
                        IdCargo = funcionario.IdCargo,
                        IdDepartamento = funcionario.IdDepartamento,
                        IdPapel = funcionario.Usuario.IdPapel,

                        IdGestor = (funcionario.Gestor != null) ? funcionario.Gestor.IdFuncionario : (int?)null,
                        NomeGestor = (funcionario.Gestor != null) ? funcionario.Gestor.Usuario.Nome : string.Empty,

                        Cargo = cargo,
                        Departamento = departamento,
                        Papel = papel,
                        Sexo = sexo,
                    };
                    return View(funcionarioCadastrarViewModel);
                }
            }
            return View();
        }

        private int? CalcularIdade(DateTime? dataNascimento)
        {
            if (dataNascimento.HasValue)
            {
                var today = DateTime.Today;
                int age = today.Year - dataNascimento.Value.Year;
                if (dataNascimento.Value > today.AddYears(-age))
                    age--;
                return age;
            }
            return null;
        }

        private string CalcularTempoFuncao(DateTime? dataFuncao)
        {
            if (dataFuncao.HasValue)
            {
                var totalMeses = GetMonthsBetween(dataFuncao.Value, DateTime.Today);
                int anos = 0;
                decimal meses = 0;
                string retorno = string.Empty;

                if (totalMeses > 11)
                {
                    decimal anosFracao = (decimal)totalMeses / 12;
                    anos = (int)Math.Truncate((double)anosFracao);
                    if (anos != 0)
                    {
                        retorno += anos.ToString();
                        retorno += (anos > 1) ? " anos " : " ano ";
                    }
                    meses = Math.Round(((decimal)anos - anosFracao) * 12, 1);
                    meses = (meses < 0) ? meses * -1 : meses;//remocao do sinal
                    if (meses != 0)
                    {
                        retorno += ((int)meses).ToString();
                        retorno += (meses > 1) ? " meses " : " mês ";
                    }
                }
                else if (totalMeses == 0)
                {
                    var totalDias = DateTime.Today.Subtract(dataFuncao.Value).TotalDays;
                    retorno += totalDias;
                    retorno += totalDias > 1 ? " dias " : " dia ";
                }
                else
                {
                    retorno += totalMeses;
                    retorno += (totalMeses > 1) ? " meses " : " mês ";
                }
                return retorno;
            }
            return string.Empty;
        }

        private int GetMonthsBetween(DateTime from, DateTime to)
        {
            if (from > to) // If from > to, invert
            {
                var temp = from;
                from = to;
                to = temp;
            }

            var monthDiff = Math.Abs((to.Year * 12 + (to.Month - 1)) - (from.Year * 12 + (from.Month - 1)));

            if (from.AddMonths(monthDiff) > to || to.Day < from.Day)
            {
                return monthDiff - 1;
            }
            else
            {
                return monthDiff;
            }
        }
        #endregion
    }
}
