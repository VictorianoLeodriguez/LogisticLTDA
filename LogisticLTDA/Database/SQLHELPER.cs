using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LogisticLTDA.Database
{
    public enum TipoCommandoSQL
    {
        Insert,
        Update,
        Delete,
        Select
    }

    public class SQLHELPER
    {
        private static readonly string _connectionString = "Server=127.0.0.1;Database=LogisticaLTDA;User Id=root;Password=03062005;";

        public static Object ExecuteCommand(TipoCommandoSQL tipoCommandoSQL, SqlCommand sqlcommand, out string errorMsg)
        {
            errorMsg = string.Empty;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    sqlcommand.Connection = connection;
                    connection.Open();

                    switch (tipoCommandoSQL)
                    {
                        case TipoCommandoSQL.Insert:
                        case TipoCommandoSQL.Update:
                        case TipoCommandoSQL.Delete:
                            return sqlcommand.ExecuteNonQuery();

                        case TipoCommandoSQL.Select:
                            DataTable sql = new DataTable();
                            using (SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand))
                            {
                                adapter.Fill(sql);
                            }
                            return sql;
                        default:
                            throw new InvalidOperationException("Tipo de Comando Invalido, Revisar;");
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                errorMsg = sqlEx.Message;
                return null;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return null;
            }
        }

        private static string TratarExcecao(SqlException sqlException)
        {
            switch (sqlException.Number)
            {
                case 2627: // Unique constraint error
                    return "Erro: Violação de chave única.";
                case 547: // Constraint check violation
                    return "Erro: Violação de restrição de integridade referencial.";
                case 2601: // Duplicated key row error
                    return "Erro: Linha duplicada.";
                default:
                    return $"Erro SQL desconhecido: {sqlException.Message}";
            }
        }

    }
}