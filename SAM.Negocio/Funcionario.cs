using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Linq;

namespace SAM.Negocio
{
    public class Funcionario
    {
        public IEnumerable<POCO.Funcionario> Listar(int? idEmpresa, int? idFuncionario = null)
        {
            using (var t = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                var funcionarios = new Dados.Funcionario().Listar(idEmpresa, idFuncionario);

                foreach (var item in funcionarios)
                {
                    item.Usuario = new Usuario().Listar(null, item.IdUsuario).FirstOrDefault();

                    if (item.IdCargo.HasValue)
                        item.Cargo = new Cargo().Listar(null, item.IdCargo).FirstOrDefault();
                    if (item.IdDepartamento.HasValue)
                        item.Departamento = new Departamento().Listar(null, item.IdDepartamento).FirstOrDefault();
                    if (item.IdGestor.HasValue)
                        item.Gestor = ListarGestor(item.IdGestor.Value);

                    yield return item;
                }
            }
        }
        public POCO.Funcionario Listar(string login)
        {
            using (var t = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted }))
                return new Dados.Funcionario().Listar(login);
        }

        public POCO.Funcionario ListarGestor(int idGestor)
        {
            using (var t = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                var funcionario = new Dados.Funcionario().Listar(null, idGestor).FirstOrDefault();
                if (funcionario != null)
                {
                    funcionario.Usuario = new Usuario().Listar(null, funcionario.IdUsuario).FirstOrDefault();

                    if (funcionario.IdCargo.HasValue)
                        funcionario.Cargo = new Cargo().Listar(null, funcionario.IdCargo).FirstOrDefault();
                    if (funcionario.IdDepartamento.HasValue)
                        funcionario.Departamento = new Departamento().Listar(null, funcionario.IdDepartamento).FirstOrDefault();

                    return funcionario;
                }
                return null;
            }
        }

        public int Salvar(POCO.Funcionario funcionario)
        {
            using (var t = new TransactionScope(TransactionScopeOption.Required))
            {
                var idUsuario = new Usuario().Salvar(funcionario.Usuario);

                funcionario.IdUsuario = idUsuario;

                //if (!string.IsNullOrEmpty(funcionario.Telefone))
                //{
                //    funcionario.Telefone = new Regex(@"[a-zA-Z\-\(\)]+").Replace(funcionario.Telefone, "");
                //}

                var idFuncionario = new Dados.Funcionario().Salvar(funcionario);

                t.Complete();

                return idFuncionario;
            }
        }
    }
}
