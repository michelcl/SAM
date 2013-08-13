using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using SAM.Dados.MySql;

namespace SAM.Dados
{
    public class Empresa : UtilDB<POCO.Empresa>
    {
        public IEnumerable<POCO.Empresa> Listar(int? idEmpresa)
        {
            var query = "select * from empresa where 1=1";

            #region FILTRO
            var listaParametros = new List<MySqlParameter>();
            if (idEmpresa.HasValue)
            {
                query += " AND IdEmpresa = @IdEmpresa";
                listaParametros.Add(new MySqlParameter("@IdEmpresa", idEmpresa.Value));
            }
            #endregion

            foreach (var item in new ConexaoMySql().ExecuteReader(query, listaParametros))
                yield return Popular(item);
        }
        
        public override IEnumerable<POCO.Empresa> Listar()
        {
            throw new System.NotImplementedException();
        }

        public override int Salvar(POCO.Empresa empresa)
        {
            var query = string.Empty;
            var listaParametros = new List<MySqlParameter>();

            if (empresa.IdEmpresa == 0)
                #region INSERT
                query = @"insert into Empresa (NomeFantasia, RazaoSocial, Dominio, Ativo, Cnpj, Telefone1, Telefone2, Logo, Segmento, Site)
                                    values(@NomeFantasia, @RazaoSocial, @Dominio, @Ativo, @Cnpj, @Telefone1, @Telefone2, @Logo, @Segmento, @Site)";
                #endregion
            else
                #region UPDATE
                query = @"update Empresa 
                                set
                                    NomeFantasia = @NomeFantasia, 
                                    RazaoSocial = @RazaoSocial, 
                                    Dominio = @Dominio, 
                                    Ativo = @Ativo,
                                    Cnpj = @Cnpj, 
                                    Telefone1 = @Telefone1, 
                                    Telefone2 = @Telefone2, 
                                    Logo = @Logo,
                                    Segmento = @Segmento, 
                                    Site = @Site
                            where IdEmpresa = @IdEmpresa";
                #endregion

            #region PARAMETROS
            if (empresa.IdEmpresa != 0)
                listaParametros.Add(new MySqlParameter("@IdEmpresa", empresa.IdEmpresa));

            listaParametros.Add(new MySqlParameter("@NomeFantasia", empresa.NomeFantasia));
            listaParametros.Add(new MySqlParameter("@RazaoSocial", empresa.RazaoSocial));
            listaParametros.Add(new MySqlParameter("@Dominio", empresa.Dominio));
            listaParametros.Add(new MySqlParameter("@Ativo", empresa.Ativo));
            listaParametros.Add(new MySqlParameter("@Cnpj", empresa.Cnpj));
            listaParametros.Add(new MySqlParameter("@Telefone1", empresa.Telefone1));
            listaParametros.Add(new MySqlParameter("@Telefone2", empresa.Telefone2));
            listaParametros.Add(new MySqlParameter("@Logo", empresa.Logo));
            listaParametros.Add(new MySqlParameter("@Segmento", empresa.Segmento));
            listaParametros.Add(new MySqlParameter("@Site", empresa.Site));
            #endregion

            var idEmpresa = new ConexaoMySql().ExecuteNonQuery(query, listaParametros);

            return idEmpresa != 0 ? idEmpresa : empresa.IdEmpresa;
        }

        public bool VerificarSeDominioExiste(string dominio)
        {
            var query = "select * from empresa where 1 = 1";

            #region FILTRO
            var listaParametros = new List<MySqlParameter>();
            query += " AND Dominio = @Dominio";
            listaParametros.Add(new MySqlParameter("@Dominio", dominio));
            #endregion

            if (new ConexaoMySql().ExecuteScalar(query, listaParametros) != null)
                return true;
            return false;
        }

        protected override POCO.Empresa Popular(IDataRecord item)
        {
            return new POCO.Empresa
            {
                #region DADOS
                IdEmpresa = RetornarDado<int>(item["IdEmpresa"]),
                NomeFantasia = RetornarDado<string>(item["NomeFantasia"]),
                RazaoSocial = RetornarDado<string>(item["RazaoSocial"]),
                Dominio = RetornarDado<string>(item["Dominio"]),
                Ativo = RetornarDado<bool>(item["Ativo"]),
                Cnpj = RetornarDado<string>(item["Cnpj"]),
                Telefone1 = RetornarDado<string>(item["Telefone1"]),
                Telefone2 = RetornarDado<string>(item["Telefone2"]),
                Logo = RetornarDado<string>(item["Logo"]),
                Segmento = RetornarDado<string>(item["Segmento"]),
                Site = RetornarDado<string>(item["Site"]),
                #endregion
            };
        }
    }
}
