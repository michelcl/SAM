using System.ComponentModel.DataAnnotations;

namespace SAM.POCO
{
    public class Cargo
    {
        [Key]
        public int IdCargo{get;set;}
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public int IdEmpresa { get; set; }
    }
}
