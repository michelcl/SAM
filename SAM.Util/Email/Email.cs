using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace SAM.Util.Email
{
    public class Email
    {
        #region Propriedades
        private string Servidor { get; set; }
        private string From { get; set; }
        private string Usuario { get; set; }
        private string Senha { get; set; }
        private int Porta = 587;
        #endregion

        public Email()
        {
            Servidor = ConfigurationManager.AppSettings["ServidorSmtp"];
            Usuario = ConfigurationManager.AppSettings["UsuarioSmtp"];
            From = ConfigurationManager.AppSettings["UsuarioSmtp"];
            Senha = ConfigurationManager.AppSettings["SenhaUsuarioSmtp"];
            Porta = int.Parse(ConfigurationManager.AppSettings["PortaSmtp"]);
        }
        /// <summary>
        /// Envia o email para um usuario
        /// </summary>
        /// <param name="email"></param>
        /// <param name="assunto"></param>
        /// <param name="corpo"></param>
        public void Enviar(string email, string assunto, string corpo)
        {
            var mail = new MailMessage(From, email, assunto, corpo)
            {
                IsBodyHtml = true
            };

            var server = new SmtpClient(Servidor, Porta)
            {
                Credentials = new NetworkCredential(Usuario, Senha),
                EnableSsl = true
            };
            server.Send(mail);
        }
    }
}
