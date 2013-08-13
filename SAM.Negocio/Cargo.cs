using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Transactions;
using SAM.POCO;

namespace SAM.Negocio
{
    public class Cargo
    {
        public IEnumerable<POCO.Cargo> Listar(int? idEmpresa, int? idCargo = null)
        {
            using (var t = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted }))
                return new Dados.Cargo().Listar(idEmpresa, idCargo);
        }
    }
}
