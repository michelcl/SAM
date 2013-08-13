using System.Collections.Generic;

namespace SAM.Web.ViewModels.Comum
{
    public class UsuarioLogadoViewModel
    {
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Foto { get; set; }
        public int IdEmpresa { get; set; }
        public IEnumerable<POCO.Empresa> ListaEmpresa { get; set; }
    }
}