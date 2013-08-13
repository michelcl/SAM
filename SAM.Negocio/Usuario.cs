using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using SAM.Util.Autenticacao;
using SAM.Util.Criptografia;
using SAM.Util.Email;

namespace SAM.Negocio
{
    public class Usuario
    {
        /// <summary>
        /// Retorna o objeto Usuario de acordo com o login.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public POCO.Usuario Listar(string login)
        {
            //Query executada sem transação e com NOLOCK
            using (var t = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                var usuario = new Dados.Usuario().Listar(login);
                usuario.Empresa = new Empresa().Listar(usuario.IdEmpresa).FirstOrDefault();

                return usuario;
            }
        }

        /// <summary>
        /// Lista todos os usuários da empresa ou usuario especifico
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public IEnumerable<POCO.Usuario> Listar(int? idEmpresa, int? idUsuario = null)
        {
            using (var t = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                var usuarios = new Dados.Usuario().Listar(idEmpresa, idUsuario);

                foreach (var item in usuarios)
                {
                    item.Empresa = new Empresa().Listar(item.IdEmpresa).FirstOrDefault();
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Salva todo o objeto Usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public int Salvar(POCO.Usuario usuario)
        {
            using (var t = new TransactionScope(TransactionScopeOption.Required))
            {
                usuario.Login = RetornarLoginComDominio(usuario.Login, usuario.IdEmpresa);

                if (!string.IsNullOrEmpty(usuario.Senha))
                    usuario.Senha = PasswordHash.CreateHash(usuario.Senha);

                int idUsuario = new Dados.Usuario().Salvar(usuario);

                t.Complete();
                return idUsuario;
            }
        }

        public void RedefinirSenha(string login, string urlRedefinirSenha)
        {
            using (var t = new TransactionScope(TransactionScopeOption.Required))
            {
                var guid = Guid.NewGuid().ToString();

                var email = Listar(login).Email;
                
                var loginProtegido = new CriptografiaSimples().CriptografarParaUrl(login);
                SalvarChaveRedefinirSenha(login, guid);
                #region corpo email
                var corpo = string.Format(@"
                        Você está recebendo esse e-mail porque foi solicitado o link para redefinir senha do login: {0}
<br/>

Clique no link abaixo para redefinir sua senha:<br/>
{1}", login, urlRedefinirSenha + "/?l=" + loginProtegido + "&g=" + guid);
                #endregion

                new Email().Enviar(email, "Recuperar Senha", corpo);

                t.Complete();
            }
        }

        public POCO.Usuario ValidarRedefinirSenha(string loginCriptografado, string guid)
        {
            var login = new CriptografiaSimples().Descriptografar(loginCriptografado);

            var usuario = Listar(login);

            if (usuario != null && !string.IsNullOrEmpty(usuario.ChaveRedefinirSenha))
                if (Util.Autenticacao.PasswordHash.ValidatePassword(guid, usuario.ChaveRedefinirSenha))
                    return usuario;

            return null;//Usuario não existe na base
        }

        /// <summary>
        /// Atualiza senha do usuário.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="senha"></param>
        public void AtualizarSenha(string login, string senha)
        {
            using (var t = new TransactionScope(TransactionScopeOption.Required))
            {
                var usuario = Listar(login);
                if (usuario != null)
                {
                    usuario.Senha = senha;
                    Salvar(usuario);
                    SalvarChaveRedefinirSenha(login, "");//Após a redefinição da senha, a chave é removida.
                }
                t.Complete();
            }
        }

        private void SalvarChaveRedefinirSenha(string login, string guid)
        {
            var hashChaveRedefinirSenha = string.Empty;
            if (!string.IsNullOrEmpty(guid))
                hashChaveRedefinirSenha = Util.Autenticacao.PasswordHash.CreateHash(guid);

            new Dados.Usuario().SalvarChaveRedefinirSenha(login, hashChaveRedefinirSenha);
        }


        /// <summary>
        /// Verifica se o login passado por parametro já está sendo utilizado por outro usuário.
        /// </summary>
        /// <param name="login">login do usuario para validar</param>
        /// <returns>Boolean 
        /// True - Login existe
        /// False - Login não existe
        /// </returns>
        public bool VerificarSeLoginExiste(string login, int idEmpresa)
        {
            login = RetornarLoginComDominio(login, idEmpresa);
            return (new Dados.Usuario().Listar(login) != null);
        }

        public bool VerificarSeEmailExiste(string email)
        {
            return new Dados.Usuario().VerificarSeEmailExiste(email);
        }

        /// <summary>
        /// Retorna o Login com Dominio
        /// <para>Se o login já estiver concatenado com o dominio o login será retornado corretamente, sem duplicação do dominio.</para>
        /// </summary>
        /// <param name="login">Exemplo: josedasilva</param>
        /// <param name="idEmpresa">Id da Empresa</param>
        /// <returns>Exemplo: josedasilva@dominiodaempresa.com.br</returns>
        public string RetornarLoginComDominio(string login, int idEmpresa)
        {
            if (!string.IsNullOrEmpty(login))
            {
                var empresa = new Dados.Empresa().Listar(idEmpresa).FirstOrDefault();
                if (empresa != null)
                {
                    if (login.IndexOf("@" + empresa.Dominio) > 0)
                        login = login.Remove(login.IndexOf("@" + empresa.Dominio), ("@" + empresa.Dominio).Length);

                    return (login + "@" + empresa.Dominio).ToLower();
                }
            }
            return string.Empty;
        }
    }
}
