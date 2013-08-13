using System.ComponentModel.DataAnnotations;

namespace SAM.Web.ViewModels.Filial
{
    public class FilialVisualizarViewModel
    {
        public int IdFilial { get; set; }

        [Display(Name = "Razão Social")]
        public string RazaoSocial { get; set; }

        [Display(Name = "Nome Fantasia")]
        public string NomeFantasia { get; set; }
        public string Cnpj { get; set; }

        [Display(Name = "Telefone 1")]
        public string Telefone1 { get; set; }

        [Display(Name = "Telefone 2")]
        public string Telefone2 { get; set; }
        public bool Ativo { get; set; }

        #region Dados Empresa
        public int IdEmpresa { get; set; }
        public string NomeFantasiaEmpresa { get; set; }
        #endregion
    }
}