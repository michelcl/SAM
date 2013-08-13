using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SAM.Dados.MySql;

namespace SAM.Dados
{
    public class Filial : UtilDB<POCO.Filial>
    {
        public IEnumerable<POCO.Filial> Listar(int? idEmpresa = null, int? idFilial = null)
        {
            var query = "select * from filial where 1=1";

            #region FILTRO
            var listaParametros = new List<MySqlParameter>();
            if (idEmpresa.HasValue)
            {
                query += " AND IdEmpresa = @IdEmpresa";
                listaParametros.Add(new MySqlParameter("@IdEmpresa", idEmpresa.Value));
            }
            if (idFilial.HasValue)
            {
                query += " AND IdFilial = @IdFilial";
                listaParametros.Add(new MySqlParameter("@IdFilial", idFilial.Value));
            }
            #endregion

            foreach (var item in new ConexaoMySql().ExecuteReader(query, listaParametros))
                yield return Popular(item);
        }
        
        public override IEnumerable<POCO.Filial> Listar()
        {
            throw new NotImplementedException();
        }

        public override int Salvar(POCO.Filial filial)
        {
            var query = string.Empty;
            var listaParametros = new List<MySqlParameter>();

            if (filial.IdFilial == 0)
                #region INSERT
                query = @"insert into Filial (NomeFantasia, RazaoSocial, Cnpj, Telefone1, Telefone2, Ativo, IdEmpresa)
                                    values(@NomeFantasia, @RazaoSocial, @Cnpj, @Telefone1, @Telefone2, @Ativo, @IdEmpresa)";
                #endregion
            else
                #region UPDATE
                query = @"update Filial 
                                set
                                    NomeFantasia = @NomeFantasia, 
                                    RazaoSocial = @RazaoSocial, 
                                    Cnpj = @Cnpj, 
                                    Telefone1 = @Telefone1, 
                                    Telefone2 = @Telefone2, 
                                    Ativo = @Ativo,
                                    IdEmpresa = @IdEmpresa
                            where IdFilial = @IdFilial";
                #endregion

            #region PARAMETROS
            if (filial.IdEmpresa != 0)
                listaParametros.Add(new MySqlParameter("@IdFilial", filial.IdFilial));

            listaParametros.Add(new MySqlParameter("@NomeFantasia", filial.NomeFantasia));
            listaParametros.Add(new MySqlParameter("@RazaoSocial", filial.RazaoSocial));
            listaParametros.Add(new MySqlParameter("@Cnpj", filial.Cnpj));
            listaParametros.Add(new MySqlParameter("@Telefone1", filial.Telefone1));
            listaParametros.Add(new MySqlParameter("@Telefone2", filial.Telefone2));
            listaParametros.Add(new MySqlParameter("@Ativo", filial.Ativo));
            listaParametros.Add(new MySqlParameter("@IdEmpresa", filial.IdEmpresa));
            #endregion

            var idFilial = new ConexaoMySql().ExecuteNonQuery(query, listaParametros);

            return idFilial != 0 ? idFilial : filial.IdFilial;
        }

        protected override POCO.Filial Popular(IDataRecord item)
        {
            return new POCO.Filial
            {
                IdFilial = RetornarDado<int>(item["IdFilial"]),
                NomeFantasia = RetornarDado<string>(item["NomeFantasia"]),
                RazaoSocial = RetornarDado<string>(item["RazaoSocial"]),
                Cnpj = RetornarDado<string>(item["Cnpj"]),
                Telefone1 = RetornarDado<string>(item["Telefone1"]),
                Telefone2 = RetornarDado<string>(item["Telefone2"]),
                Ativo = RetornarDado<bool>(item["Ativo"]),
                IdEmpresa = RetornarDado<int>(item["IdEmpresa"]),
            };
        }
    }
}
