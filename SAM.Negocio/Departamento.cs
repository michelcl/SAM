using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Transactions;
using SAM.POCO;

namespace SAM.Negocio
{
    public class Departamento
    {
        public IEnumerable<POCO.Departamento> Listar(int? idEmpresa, int? idDepartamento = null)
        {
            using (var t = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted }))
                return new Dados.Departamento().Listar(idEmpresa, idDepartamento);
        }
    }
}
