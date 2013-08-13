using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAM.Web.ViewModels.Filial
{
    public class FilialCadastrarViewModel
    {
        public int IdFilial { get; set; }
        
        [Display(Name = "Razão Social")]
        [Required(ErrorMessage = "Razão Social é requerido.")]
        public string RazaoSocial { get; set; }
        
        [Display(Name = "Nome Fantasia")]
        public string NomeFantasia { get; set; }

        [Required(ErrorMessage = "CNPJ é requerido.")]
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