using System.ComponentModel.DataAnnotations;

namespace SAM.Web.ViewModels.Home
{
    public class EsqueciMinhaSenhaViewModel
    {
        [Required(ErrorMessage = "Login é requerido.")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Login { get; set; }
    }
}