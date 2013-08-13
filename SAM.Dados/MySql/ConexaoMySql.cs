using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SAM.Dados.MySql
{
    public class ConexaoMySql
    {
        public string RetornarConexao
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionMySql"].ToString();
            }
        }

        public IEnumerable<IDataRecord> ExecuteReader(string query, List<MySqlParameter> parametros = null)
        {
            using (var conn = new MySqlConnection(RetornarConexao))
            {
                conn.Open();
                using (var command = new MySqlCommand(query, conn))
                {
                    if (parametros != null)
                        command.Parameters.AddRange(parametros.ToArray());
                    using (IDataReader rdr = command.ExecuteReader())
                    {
                        while (rdr.Read())
                            yield return (IDataRecord)rdr;
                    }
                }
                conn.Dispose();
            }
        }

        public object ExecuteScalar(string query, List<MySqlParameter> parametros = null)
        {
            using (var conn = new MySqlConnection(RetornarConexao))
            {
                conn.Open();
                using (var command = new MySqlCommand(query, conn))
                {
                    if (parametros != null)
                        command.Parameters.AddRange(parametros.ToArray());
                    return command.ExecuteScalar();
                }
            }
        }

        public int ExecuteNonQuery(string query, List<MySqlParameter> parametros)
        {
            using (var conn = new MySqlConnection(RetornarConexao))
            {
                conn.Open();
                using (var command = new MySqlCommand(query, conn))
                {
                    command.Parameters.AddRange(parametros.ToArray());
                    command.ExecuteNonQuery();
                    return (int)command.LastInsertedId;
                }
            }
        }
    }
}
