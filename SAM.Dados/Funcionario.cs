using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using SAM.Dados.MySql;

namespace SAM.Dados
{
    public class Funcionario : UtilDB<POCO.Funcionario>
    {
        public IEnumerable<POCO.Funcionario> Listar(int? idEmpresa, int? idFuncionario)
        {
            var query = @"select f.* from Funcionario f
                          join Usuario u on u.IdUsuario = f.IdUsuario
                          where 1 = 1";

            #region FILTRO
            var listaParametros = new List<MySqlParameter>();
            if (idEmpresa.HasValue)
            {
                query += " AND IdEmpresa = @IdEmpresa";
                listaParametros.Add(new MySqlParameter("@IdEmpresa", idEmpresa.Value));
            }
            if (idFuncionario.HasValue)
            {
                query += " AND IdFuncionario = @IdFuncionario";
                listaParametros.Add(new MySqlParameter("@IdFuncionario", idFuncionario.Value));
            }

            #endregion

            foreach (var item in new ConexaoMySql().ExecuteReader(query, listaParametros))
                yield return Popular(item);
        }
        public POCO.Funcionario Listar(string login)
        {
            var query = @"select f.* from Usuario u
                            join Funcionario f on f.IdUsuario = u.IdUsuario
                            where 1 = 1";
            #region FILTRO
            var listaParametros = new List<MySqlParameter>();
            query += " AND Login = @Login";
            listaParametros.Add(new MySqlParameter("@Login", login));
            #endregion

            foreach (var item in new ConexaoMySql().ExecuteReader(query, listaParametros))
                return Popular(item);

            return null;
        }

        public override int Salvar(POCO.Funcionario funcionario)
        {
            var query = string.Empty;
            var listaParametros = new List<MySqlParameter>();

            if (funcionario.IdFuncionario == 0)
                #region INSERT
                query = @"insert into Funcionario (Telefone, Celular, Foto, Matricula, Sexo, Skype, DataNascimento, DataFuncao, IdUsuario, IdCargo, IdDepartamento, IdGestor) 
                                    values(@Telefone, @Celular, @Foto, @Matricula, @Sexo, @Skype, @DataNascimento, @DataFuncao, @IdUsuario, @IdCargo, @IdDepartamento, @IdGestor)";
                #endregion
            else
                #region UPDATE
                query = @"update Funcionario 
                                set
                                Telefone = @Telefone,
                                Celular = @Celular,
                                Foto = @Foto,
                                Matricula = @Matricula,
                                Sexo = @Sexo,
                                Skype = @Skype,
                                DataNascimento = @DataNascimento,
                                DataFuncao = @DataFuncao,
                                IdUsuario = @IdUsuario,
                                IdCargo = @IdCargo,
                                IdDepartamento = @IdDepartamento,
                                IdGestor = @IdGestor
                            where IdFuncionario = @IdFuncionario";
                #endregion

            #region PARAMETROS
            if (funcionario.IdFuncionario != 0)
                listaParametros.Add(new MySqlParameter("@IdFuncionario", funcionario.IdFuncionario));

            listaParametros.Add(new MySqlParameter("@Telefone", funcionario.Telefone));
            listaParametros.Add(new MySqlParameter("@Celular", funcionario.Celular));
            listaParametros.Add(new MySqlParameter("@Foto", funcionario.Foto));

            listaParametros.Add(new MySqlParameter("@Matricula", funcionario.Matricula));
            listaParametros.Add(new MySqlParameter("@Sexo", funcionario.Sexo));
            listaParametros.Add(new MySqlParameter("@Skype", funcionario.Skype));
            listaParametros.Add(new MySqlParameter("@DataNascimento", funcionario.DataNascimento));
            listaParametros.Add(new MySqlParameter("@DataFuncao", funcionario.DataFuncao));

            listaParametros.Add(new MySqlParameter("@IdUsuario", funcionario.IdUsuario));
            listaParametros.Add(new MySqlParameter("@IdCargo", funcionario.IdCargo));
            listaParametros.Add(new MySqlParameter("@IdDepartamento", funcionario.IdDepartamento));
            listaParametros.Add(new MySqlParameter("@IdGestor", funcionario.IdGestor));
            #endregion

            var idFuncionario = new ConexaoMySql().ExecuteNonQuery(query, listaParametros);

            return idFuncionario != 0 ? idFuncionario : funcionario.IdFuncionario;
        }

        public override IEnumerable<POCO.Funcionario> Listar()
        {
            throw new System.NotImplementedException();
        }

        protected override POCO.Funcionario Popular(IDataRecord item)
        {
            return new POCO.Funcionario
            {
                #region DADOS
                IdFuncionario = RetornarDado<int>(item["IdFuncionario"]),
                Telefone = RetornarDado<string>(item["Telefone"]),
                Celular = RetornarDado<string>(item["Celular"]),
                Foto = RetornarDado<string>(item["Foto"]),
                Matricula = RetornarDado<string>(item["Matricula"]),
                Sexo = RetornarDado<string>(item["Sexo"]),
                Skype = RetornarDado<string>(item["Skype"]),
                DataNascimento = RetornarDado<DateTime?>(item["DataNascimento"]),
                DataFuncao = RetornarDado<DateTime?>(item["DataFuncao"]),

                IdCargo = RetornarDado<int?>(item["IdCargo"]),
                IdDepartamento = RetornarDado<int?>(item["IdDepartamento"]),
                IdUsuario = RetornarDado<int>(item["IdUsuario"]),
                IdGestor = RetornarDado<int?>(item["IdGestor"]),
                #endregion
            };
        }
    }
}
