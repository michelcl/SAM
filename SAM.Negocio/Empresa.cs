using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using SAM.POCO;

namespace SAM.Negocio
{
    public class Empresa
    {
        public IEnumerable<POCO.Empresa> Listar(int? idEmpresa = null)
        {
            using (var t = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted }))
                return new Dados.Empresa().Listar(idEmpresa);
        }

        public int Salvar(POCO.Empresa empresa)
        {
            #region remocao formatacao
            if (!string.IsNullOrEmpty(empresa.Cnpj))
                empresa.Cnpj = empresa.Cnpj.Replace("/", "").Replace(".", "").Replace("-", "");
            if (!string.IsNullOrEmpty(empresa.Telefone1))
                empresa.Telefone1 = new Regex(@"[a-zA-Z\-\(\)]+").Replace(empresa.Telefone1, "");
            if (!string.IsNullOrEmpty(empresa.Telefone2))
                empresa.Telefone2 = new Regex(@"[a-zA-Z\-\(\)]+").Replace(empresa.Telefone2, "");
            #endregion

            using (var t = new TransactionScope(TransactionScopeOption.Required))
            {
                var idEmpresa = new Dados.Empresa().Salvar(empresa);
                t.Complete();
                return idEmpresa;
            }
        }

        public bool VerificarSeDominioExiste(string dominio)
        {
            return new Dados.Empresa().VerificarSeDominioExiste(dominio);
        }

    }
}
