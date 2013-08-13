using System.Security.Principal;

namespace SAM.Web.Autenticacao
{
    interface IUsuarioAutenticado
    {
        int IdUsuario { get; set; }
        string Login { get; set; }
        string Nome { get; set; }
        int IdEmpresa { get; set; }
    }
}
