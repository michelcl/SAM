using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using SAM.Dados.MySql;

namespace SAM.Dados
{
    public class Departamento : UtilDB<POCO.Departamento>
    {
        public IEnumerable<POCO.Departamento> Listar(int? idEmpresa, int? idDepartamento = null)
        {
            var query = "select * from departamento where Ativo = 1";

            #region FILTRO
            var listaParametros = new List<MySqlParameter>();
            if (idEmpresa.HasValue)
            {
                query += " AND IdEmpresa = @IdEmpresa";
                listaParametros.Add(new MySqlParameter("@IdEmpresa", idEmpresa.Value));
            }
            if (idDepartamento.HasValue)
            {
                query += " AND IdDepartamento = @IdDepartamento";
                listaParametros.Add(new MySqlParameter("@IdDepartamento", idDepartamento.Value));
            }
            #endregion

            foreach (var item in new ConexaoMySql().ExecuteReader(query, listaParametros))
                yield return Popular(item);
        }


        public override IEnumerable<POCO.Departamento> Listar()
        {
            throw new System.NotImplementedException();
        }

        public override int Salvar(POCO.Departamento objeto)
        {
            throw new System.NotImplementedException();
        }

        protected override POCO.Departamento Popular(IDataRecord item)
        {
            return new POCO.Departamento
            {
                #region DADOS
                IdDepartamento = RetornarDado<int>(item["IdDepartamento"]),
                Descricao = RetornarDado<string>(item["Descricao"]),
                Ativo = RetornarDado<bool>(item["Ativo"]),
                IdEmpresa = RetornarDado<int>(item["IdEmpresa"])
                #endregion
            };
        }
    }
}
