using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAM.Web.Autenticacao
{
    public class UsuarioAutenticadoSerializeModel : IUsuarioAutenticado
    {
        public int IdUsuario { get; set; }
        public string Login { get; set; }        
        public string Nome { get; set; }
        public int IdEmpresa { get; set; }
    }
}