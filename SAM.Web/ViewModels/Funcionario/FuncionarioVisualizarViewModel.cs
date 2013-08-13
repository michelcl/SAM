using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAM.Web.ViewModels.Funcionario
{
    public class FuncionarioVisualizarViewModel
    {
        public int IdFuncionario { get; set; }
        public string Nome { get; set; }

        public string Login { get; set; }

        public string LoginInicial { get; set; }

        public string Senha { get; set; }
        public string Email { get; set; }

        public string EmailInicial { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public bool Ativo { get; set; }

        public string Foto { get; set; }

        public string Matricula { get; set; }

        public string Sexo { get; set; }

        public string Skype { get; set; }

        [Display(Name = "Data do Nascimento"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataNascimento { get; set; }
        public int? Idade { get; set; }
        
        [Display(Name = "Data da Função"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataFuncao { get; set; }

        [Display(Name = "Tempo na função")]
        public string TempoFuncao { get; set; }

        public string IdEmpresa { get; set; }

        [Display(Name = "Empresa")]
        public string NomeEmpresa { get; set; }
        public string DominioEmpresa { get; set; }

        public int IdUsuario { get; set; }
        public int? IdCargo { get; set; }
        public int? IdDepartamento { get; set; }

        public int? IdGestor { get; set; }

        [Display(Name = "Nome do Gestor")]
        public string NomeGestor { get; set; }

        public int? IdPapel { get; set; }

        public string Cargo { get; set; }
        public string Departamento { get; set; }
        public string Papel { get; set; }
    }
}