using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using SAM.Dados.MySql;

namespace SAM.Dados
{
    public class Usuario : UtilDB<POCO.Usuario>
    {
        public IEnumerable<POCO.Usuario> Listar(int? idEmpresa, int? idUsuario)
        {
            var query = "select * from usuario where 1 = 1";

            #region FILTRO
            var listaParametros = new List<MySqlParameter>();
            if (idEmpresa.HasValue)
            {
                query += " AND IdEmpresa = @IdEmpresa";
                listaParametros.Add(new MySqlParameter("@IdEmpresa", idEmpresa.Value));
            }
            if (idUsuario.HasValue)
            {
                query += " AND IdUsuario = @IdUsuario";
                listaParametros.Add(new MySqlParameter("@IdUsuario", idUsuario.Value));
            }

            #endregion

            foreach (var item in new ConexaoMySql().ExecuteReader(query, listaParametros))
                yield return Popular(item);
        }

        public POCO.Usuario Listar(string login)
        {
            var query = "select * from usuario where 1 = 1";

            #region FILTRO
            var listaParametros = new List<MySqlParameter>();
            query += " AND Login = @Login";
            listaParametros.Add(new MySqlParameter("@Login", login));
            #endregion

            foreach (var item in new ConexaoMySql().ExecuteReader(query, listaParametros))
                return Popular(item);

            return null;
        }

        public bool VerificarSeEmailExiste(string email)
        {
            var query = "select * from usuario where Ativo = 1";

            #region FILTRO
            var listaParametros = new List<MySqlParameter>();
            query += " AND Email = @Email";
            listaParametros.Add(new MySqlParameter("@Email", email));
            #endregion

            if (new ConexaoMySql().ExecuteScalar(query, listaParametros) != null)
                return true;
            return false;
        }

        public override int Salvar(POCO.Usuario usuario)
        {
            var query = string.Empty;
            var listaParametros = new List<MySqlParameter>();

            if (usuario.IdUsuario == 0)
                #region INSERT
                query = @"insert into Usuario (Nome, Login, Senha, Email, Ativo, IdEmpresa, IdPapel) 
                                    values(@Nome, @Login, @Senha, @Email, @Ativo, @IdEmpresa, @IdPapel)";
                #endregion
            else
                #region UPDATE
                query = @"update Usuario 
                                set
                                Nome = @Nome,
                                Login = @Login, 
                                Senha = coalesce(@Senha, Senha),
                                Email = @Email,
                                Ativo = @Ativo,
                                IdEmpresa = @IdEmpresa,
                                IdPapel = @IdPapel
                            where IdUsuario = @IdUsuario";
                #endregion

            #region PARAMETROS
            if (usuario.IdUsuario != 0)
                listaParametros.Add(new MySqlParameter("@IdUsuario", usuario.IdUsuario));

            listaParametros.Add(new MySqlParameter("@Nome", usuario.Nome));
            listaParametros.Add(new MySqlParameter("@Login", usuario.Login));
            listaParametros.Add(new MySqlParameter("@Senha", usuario.Senha));
            listaParametros.Add(new MySqlParameter("@Email", usuario.Email));
            listaParametros.Add(new MySqlParameter("@Ativo", usuario.Ativo));
            listaParametros.Add(new MySqlParameter("@IdEmpresa", usuario.IdEmpresa));
            listaParametros.Add(new MySqlParameter("@IdPapel", usuario.IdPapel));
            #endregion

            var idUsuario = new ConexaoMySql().ExecuteNonQuery(query, listaParametros);

            return idUsuario != 0 ? idUsuario : usuario.IdUsuario;
        }

        public void SalvarChaveRedefinirSenha(string login, string chaveRedefinirSenha)
        {
            #region UPDATE
            var query = @"update Usuario 
                          set ChaveRedefinirSenha = @ChaveRedefinirSenha
                          where Login = @Login";
            #endregion

            #region PARAMETROS
            var listaParametros = new List<MySqlParameter>();
            listaParametros.Add(new MySqlParameter("@Login", login));
            listaParametros.Add(new MySqlParameter("@ChaveRedefinirSenha", chaveRedefinirSenha));
            #endregion
            new ConexaoMySql().ExecuteNonQuery(query, listaParametros);
        }

        public override IEnumerable<POCO.Usuario> Listar()
        {
            throw new NotImplementedException();
        }

        protected override POCO.Usuario Popular(IDataRecord item)
        {
            return new POCO.Usuario
            {
                #region DADOS
                IdUsuario = RetornarDado<int>(item["IdUsuario"]),

                Nome = RetornarDado<string>(item["Nome"]),
                Login = RetornarDado<string>(item["Login"]),
                Email = RetornarDado<string>(item["Email"]),
                Senha = RetornarDado<string>(item["Senha"]),
                ChaveRedefinirSenha = RetornarDado<string>(item["ChaveRedefinirSenha"]),
                Ativo = RetornarDado<bool>(item["Ativo"]),
                IdEmpresa = RetornarDado<int>(item["IdEmpresa"]),
                IdPapel = RetornarDado<int?>(item["IdPapel"]),
                #endregion
            };
        }
    }
}
