using System.ComponentModel.DataAnnotations;

namespace SAM.POCO
{
    public class Papel
    {
        [Key]
        public int IdPapel { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
    }
}
