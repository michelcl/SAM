using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using SAM.Dados.MySql;

namespace SAM.Dados
{
    public class Papel : UtilDB<POCO.Papel>
    {
        public IEnumerable<POCO.Papel> Listar(string login)
        {
            var query = @"select p.* from usuario u
                          join papel p on p.IdPapel = u.IdPapel
                          where p.Ativo = 1";
            #region FILTRO
            var listaParametros = new List<MySqlParameter>();
            query += " AND Login = @Login";
            listaParametros.Add(new MySqlParameter("@Login", login));
            #endregion

            foreach (var item in new ConexaoMySql().ExecuteReader(query, listaParametros))
                yield return Popular(item);
        }

        public override IEnumerable<POCO.Papel> Listar()
        {
            var query = @"select * from papel where Ativo = 1";

            foreach (var item in new ConexaoMySql().ExecuteReader(query))
                yield return Popular(item);
        }
        
        public override int Salvar(POCO.Papel objeto)
        {
            throw new System.NotImplementedException();
        }

        protected override POCO.Papel Popular(IDataRecord item)
        {
            return new POCO.Papel
            {
                #region DADOS
                IdPapel = RetornarDado<int>(item["IdPapel"]),
                Descricao = RetornarDado<string>(item["Descricao"]),
                Ativo = RetornarDado<bool>(item["Ativo"])
                #endregion
            };
        }
    }
}
