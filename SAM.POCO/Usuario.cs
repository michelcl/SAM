using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAM.POCO
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public string ChaveRedefinirSenha { get; set; }
        public bool Ativo { get; set; }

        public int IdEmpresa { get; set; }
        public int? IdPapel { get; set; }

        public Empresa Empresa { get; set; }
    }
}
