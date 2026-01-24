using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LogisticLTDA.Database
{
    public class DBSqlServer
    {

        public enum QueryMode
        {
            Insert = 0,
            Edit = 1,
            Delete = 2
        }

        public static SqlConnection AbreConexao(out string errorMsg)
        {
            try
            {
                SqlConnection conexao = new SqlConnection
                {
                    ConnectionString = ConfigurationManager
                        .ConnectionStrings["DataBase"]
                        .ConnectionString
                };

                conexao.Open();
                errorMsg = string.Empty;
                return conexao;
            }
            catch (Exception ex)
            {
                errorMsg = "Erro ao conectar no banco de dados:" +
                           Environment.NewLine + ex.Message;
                return null;
            }
        }

        public static void FecharConexao(SqlConnection conexao, out string errorMsg)
        {
            errorMsg = string.Empty;

            try
            {
                if (conexao == null || conexao.State == ConnectionState.Closed)
                    return;

                conexao.Close();
            }
            catch (Exception ex)
            {
                errorMsg = "Erro ao fechar conexão:" +
                           Environment.NewLine + ex.Message;
            }
            finally
            {
                conexao?.Dispose();
            }
        }

        public static List<Dictionary<string, string>> Consulta(
            SqlCommand sqlCommand,
            out string errorMsg)
        {
            errorMsg = string.Empty;
            SqlConnection conexao = null;
            List<Dictionary<string, string>> lista = null;

            try
            {
                conexao = AbreConexao(out errorMsg);
                if (conexao == null) return null;

                sqlCommand.Connection = conexao;

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    lista = new List<Dictionary<string, string>>();

                    while (reader.Read())
                    {
                        var linha = new Dictionary<string, string>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string coluna = reader.GetName(i);
                            string valor = reader.IsDBNull(i)
                                ? null
                                : reader.GetValue(i).ToString();

                            linha.Add(coluna, valor);
                        }

                        lista.Add(linha);
                    }
                }

                if (lista.Count == 0)
                    return null;
            }
            catch (Exception ex)
            {
                errorMsg = "Erro na consulta:" +
                           Environment.NewLine + ex.Message;
            }
            finally
            {
                string msg = errorMsg;
                FecharConexao(conexao, out errorMsg);
                errorMsg = string.IsNullOrEmpty(errorMsg)
                    ? msg
                    : msg + Environment.NewLine + errorMsg;
            }

            return lista;
        }

        public static Dictionary<string, string> ConsultaObjeto(
            SqlCommand sqlCommand,
            out string errorMsg,
            SqlConnection conexao = null)
        {
            errorMsg = string.Empty;
            Dictionary<string, string> obj = null;

            try
            {
                if (conexao == null)
                {
                    conexao = AbreConexao(out errorMsg);
                    if (conexao == null) return null;
                }

                sqlCommand.Connection = conexao;

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        obj = new Dictionary<string, string>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string coluna = reader.GetName(i);
                            string valor = reader.IsDBNull(i)
                                ? null
                                : reader.GetValue(i).ToString();

                            obj.Add(coluna, valor);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = "Erro ao consultar objeto:" +
                           Environment.NewLine + ex.Message;
            }
            finally
            {
                if (conexao != null)
                {
                    string msg = errorMsg;
                    FecharConexao(conexao, out errorMsg);
                    errorMsg = string.IsNullOrEmpty(errorMsg)
                        ? msg
                        : msg + Environment.NewLine + errorMsg;
                }
            }

            return obj;
        }

        public static bool Executar(
            SqlCommand sqlCommand,
            QueryMode mode,
            out string errorMsg,
            out int insertedCode)
        {
            errorMsg = string.Empty;
            insertedCode = 0;
            SqlConnection conexao = null;

            try
            {
                conexao = AbreConexao(out errorMsg);
                if (conexao == null) return false;

                sqlCommand.Connection = conexao;
                sqlCommand.ExecuteNonQuery();

                if (mode == QueryMode.Insert)
                {
                    sqlCommand.CommandText = "SELECT SCOPE_IDENTITY()";
                    insertedCode = Convert.ToInt32(sqlCommand.ExecuteScalar());
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMsg = "Erro ao executar comando SQL:" +
                           Environment.NewLine + ex.Message;
                return false;
            }
            finally
            {
                string msg = errorMsg;
                FecharConexao(conexao, out errorMsg);
                errorMsg = string.IsNullOrEmpty(errorMsg)
                    ? msg
                    : msg + Environment.NewLine + errorMsg;
            }
        }
    }
}
