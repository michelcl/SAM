using System.ComponentModel.DataAnnotations;

namespace SAM.Web.ViewModels.Home
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Login é requerido.")]
        [EmailAddress(ErrorMessage="Formato do login inválido")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Senha é requerido.")]
        public string Senha { get; set; }
    }
}