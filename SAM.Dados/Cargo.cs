using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using SAM.Dados.MySql;

namespace SAM.Dados
{
    public class Cargo : UtilDB<POCO.Cargo>
    {
        public IEnumerable<POCO.Cargo> Listar(int? idEmpresa, int? idCargo = null)
        {
            var query = "select * from cargo where Ativo = 1";

            #region FILTRO
            var listaParametros = new List<MySqlParameter>();
            if (idEmpresa.HasValue)
            {
                query += " AND IdEmpresa = @IdEmpresa";
                listaParametros.Add(new MySqlParameter("@IdEmpresa", idEmpresa.Value));
            }
            if (idCargo.HasValue)
            {
                query += " AND IdCargo = @IdCargo";
                listaParametros.Add(new MySqlParameter("@IdCargo", idCargo.Value));
            }
            #endregion

            foreach (var item in new ConexaoMySql().ExecuteReader(query, listaParametros))
                yield return Popular(item);
        }
        
        public override IEnumerable<POCO.Cargo> Listar()
        {
            throw new System.NotImplementedException();
        }

        public override int Salvar(POCO.Cargo objeto)
        {
            throw new System.NotImplementedException();
        }

        protected override POCO.Cargo Popular(IDataRecord item)
        {
            return new POCO.Cargo
            {
                #region DADOS
                IdCargo = RetornarDado<int>(item["IdCargo"]),
                Descricao = RetornarDado<string>(item["Descricao"]),
                Ativo = RetornarDado<bool>(item["Ativo"]),
                IdEmpresa = RetornarDado<int>(item["IdEmpresa"])
                #endregion
            };
        }
    }
}
