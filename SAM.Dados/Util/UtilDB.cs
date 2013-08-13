using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.Dados
{
    /// <summary>
    /// Todas as classes de dados que fazem acesso ao banco deverão herdar a class UtilDB
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UtilDB<T>
    {
        /// <summary>
        /// Retorna o dado Tipado.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dado"></param>
        /// <returns></returns>
        public Z RetornarDado<Z>(object dado)
        {
            if (dado != DBNull.Value)
                return (Z)dado;
            else
                return default(Z);
        }

        /// <summary>
        /// Método deverá listar tudo
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<T> Listar();

        /// <summary>
        /// Salva o objeto no banco.
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public abstract int Salvar(T objeto);

        /// <summary>
        /// Popula o objeto de dados POCO.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected abstract T Popular(IDataRecord item);
    }
}
