using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SAM.POCO;
using SAM.Web.Customizacao.ValidacaoFormulario;

namespace SAM.Web.ViewModels.Funcionario
{
    public class FuncionarioCadastrarViewModel
    {
        public int IdFuncionario { get; set; }

        [Required(ErrorMessage = "Nome é requerido.")]
        public string Nome { get; set; }

        [Remote("VerificarSeLoginExiste", "Funcionario", AdditionalFields = "LoginInicial,IdEmpresa", HttpMethod = "POST", ErrorMessage = "Login já está sendo utilizado. Por favor, digite outro login.")]
        [Required(ErrorMessage = "Login é requerido.")]
        public string Login { get; set; }

        public string LoginInicial { get; set; }

        public string Senha { get; set; }

        [Required(ErrorMessage = "Email é requerido.")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [MaxLength(30, ErrorMessage = "Tamanho máximo de 30 caracteres.")]
        public string Telefone { get; set; }

        [MaxLength(30, ErrorMessage = "Tamanho máximo de 30 caracteres.")]
        public string Celular { get; set; }
        public bool Ativo { get; set; }

        public string Foto { get; set; }

        [MaxLength(20, ErrorMessage = "Tamanho máximo de 20 caracteres.")]
        public string Matricula { get; set; }

        public string Sexo { get; set; }

        [MaxLength(45, ErrorMessage = "Tamanho máximo de 20 caracteres.")]
        public string Skype { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Data de Nascimento"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataNascimento { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Data da Função"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataFuncao { get; set; }

        public string IdEmpresa { get; set; }
        public string NomeEmpresa { get; set; }
        public string DominioEmpresa { get; set; }

        public int IdUsuario { get; set; }
        public int? IdCargo { get; set; }
        public int? IdDepartamento { get; set; }
        
        [DiferenteAttribute("IdFuncionario", ErrorMessage = "O colaborador não pode ser gestor dele mesmo.")]
        public int? IdGestor { get; set; }

        [Display(Name = "Gestor")]
        public string NomeGestor { get; set; }

        //[Required(ErrorMessage = "Papel é requerido para acesso básico ao sistema.")]
        public int? IdPapel { get; set; }

        public IEnumerable<Cargo> ListaCargo { get; set; }
        public IEnumerable<Departamento> ListaDepartamento { get; set; }
        public IEnumerable<Papel> ListaPapel { get; set; }
        public IEnumerable<Sexo> ListaSexo { get; set; }            
    }
}