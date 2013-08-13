using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAM.Web.ViewModels.Empresa
{
    public class EmpresaVisualizarViewModel
    {
        public int IdEmpresa { get; set; }

        [Display(Name = "Razão Social")]
        public string RazaoSocial { get; set; }

        [Display(Name = "Nome Fantasia")]
        public string NomeFantasia { get; set; }

        [Display(Name = "Domínio")]
        public string Dominio { get; set; }
        public string DominioInicial { get; set; }

        public string Site { get; set; }

        [Display(Name = "CNPJ")]
        public string Cnpj { get; set; }

        public string Segmento { get; set; }

        public bool Ativo { get; set; }

        [Display(Name = "Telefone 1")]
        public string Telefone1 { get; set; }

        [Display(Name = "Telefone 2")]
        public string Telefone2 { get; set; }
    }
}