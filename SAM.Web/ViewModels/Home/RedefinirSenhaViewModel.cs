using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAM.Web.ViewModels.Home
{
    public class RedefinirSenhaViewModel
    {
        public string Guid { get; set; }
        public string Login { get; set; }

        [Required(ErrorMessage = "Senha é requerido.")]
        [DataType(DataType.Password)]
        [Display(Name = "Nova Senha")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Confirmar Senha é requerido.")]
        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessage="As senhas digitadas não são iguais.")]
        [Display(Name="Confirmar Nova Senha")]
        public string ConfirmarSenha { get; set; }
    }
}