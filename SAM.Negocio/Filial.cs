using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;

namespace SAM.Negocio
{
    public class Filial
    {
        public IEnumerable<POCO.Filial> Listar(int? idEmpresa = null, int? idFilial = null)
        {
            using (var t = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                var filial = new Dados.Filial().Listar(idEmpresa, idFilial);

                foreach (var item in filial)
                {
                    item.Empresa = new Empresa().Listar(item.IdEmpresa).FirstOrDefault(); ;
                    yield return item;
                }
            }
        }

        public int Salvar(POCO.Filial filial)
        {
            #region remocao formatacao
            if (!string.IsNullOrEmpty(filial.Cnpj))
                filial.Cnpj = filial.Cnpj.Replace("/", "").Replace(".", "").Replace("-", "");
            if (!string.IsNullOrEmpty(filial.Telefone1))
                filial.Telefone1 = new Regex(@"[a-zA-Z\-\(\)]+").Replace(filial.Telefone1, "");
            if (!string.IsNullOrEmpty(filial.Telefone2))
                filial.Telefone2 = new Regex(@"[a-zA-Z\-\(\)]+").Replace(filial.Telefone2, "");
            #endregion
            using (var t = new TransactionScope(TransactionScopeOption.Required))
            {
                var idFilial = new Dados.Filial().Salvar(filial);
                t.Complete();
                return idFilial;
            }
        }
    }
}