using System.Collections.Generic;

namespace SAM.Negocio
{
    public class Sexo
    {
        public IEnumerable<POCO.Sexo> Listar()
        {
            var listaSexo = new List<POCO.Sexo>();
            listaSexo.Add(new POCO.Sexo() { SiglaSexo = "M", Descricao = "Masculino" });
            listaSexo.Add(new POCO.Sexo() { SiglaSexo = "F", Descricao = "Feminino" });

            return listaSexo;
        }
    }
}
