using System.ComponentModel.DataAnnotations;

namespace SAM.POCO
{
    public class Departamento
    {
        [Key]
        public int IdDepartamento { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public int IdEmpresa { get; set; }
    }
}
