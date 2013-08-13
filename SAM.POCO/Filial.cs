
namespace SAM.POCO
{
    public class Filial
    {
        public int IdFilial { get; set; }
        public string NomeFantasia { get; set; }
        public string RazaoSocial { get; set; }
        public string Cnpj { get; set; }
        public string Telefone1 { get; set; }
        public string Telefone2 { get; set; }
        public bool Ativo { get; set; }
        public int IdEmpresa { get; set; }
        public POCO.Empresa Empresa { get; set; }
    }
}
