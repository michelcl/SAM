using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAM.Web.ViewModels.Empresa
{
    public class EmpresaCadastrarViewModel
    {
        public int IdEmpresa { get; set; }
        
        [Display(Name = "Razão Social")]
        [Required(ErrorMessage = "Razão social é requerido.")]
        public string RazaoSocial { get; set; }

        [Display(Name = "Nome Fantasia")]
        [Required(ErrorMessage = "Nome fantasia é requerido.")]
        public string NomeFantasia { get; set; }
        
        [Display(Name = "Domínio")]
        [Required(ErrorMessage = "Domínio é requerido.")]
        [RegularExpression(@"([0-9a-zA-Z]+(\.[a-zA-Z]{2,4})+)", ErrorMessage = "Domínio deve ser preenchido seguindo exemplo: nomedaempresa.com.br")]
        [Remote("VerificarSeDominioExiste", "Empresa", AdditionalFields = "DominioInicial", HttpMethod = "POST", ErrorMessage = "Domínio já está sendo utilizado. Por favor, digite outro.")]
        public string Dominio { get; set; }
        public string DominioInicial { get; set; }

        public string Site { get; set; }

        [Display(Name = "CNPJ")]
        [Required(ErrorMessage = "CNPJ é requerido.")]
        public string Cnpj { get; set; }

        public string Segmento { get; set; }

        public bool Ativo { get; set; }
        
        [Display(Name = "Telefone 1")]
        public string Telefone1 { get; set; }

        [Display(Name = "Telefone 2")]
        public string Telefone2 { get; set; }
    }
}