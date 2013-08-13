using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SAM.Negocio
{
    public class Papel
    {
        public IEnumerable<POCO.Papel> Listar(string login)
        {
            using (var t = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted }))
                return new Dados.Papel().Listar(login);
        }

        public IEnumerable<POCO.Papel> Listar()
        {
            using (var t = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted }))
                return new Dados.Papel().Listar();
        }
    }
}