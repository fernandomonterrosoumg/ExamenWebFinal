using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ExamenWebFinal.Helpers
{
    public class OracleDbManager
    {

        IConfiguration configuration;
        public OracleDbManager(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public IDbConnection GetConnection()
        {
            var connectionString = configuration.GetSection("ConnectionStrings")["oracle"];
            IDbConnection connection = new OracleConnection(connectionString);
            connection.Open();
            return connection;
        }

        public string GetConnectionString()
        {
            string connectionString = configuration.GetSection("ConnectionStrings")["oracle"];
            return connectionString;
        }

        public async Task<IEnumerable<T>> DapperExecuteQuery<T>(string sqlQuery, Dictionary<string, object> parameters = null, IDbTransaction transaction = null) where T : new()
        {
            if (transaction != null)
            {
                // Usar la transacción existente y la conexión asociada
                if (parameters != null)
                {
                    return await transaction.Connection.QueryAsync<T>(sqlQuery, parameters, transaction);
                }
                else
                {
                    return await transaction.Connection.QueryAsync<T>(sqlQuery, null, transaction);
                }
            }
            else
            {
                // No se pasó una transacción, manejar la conexión aquí
                using (OracleConnection connection = new OracleConnection(GetConnectionString()))
                {
                    await connection.OpenAsync();
                    if (parameters != null)
                    {
                        return await connection.QueryAsync<T>(sqlQuery, parameters);
                    }
                    else
                    {
                        return await connection.QueryAsync<T>(sqlQuery);
                    }
                }
            }
        }

        public async Task DapperExecuteInsertReturn(string sqlQuery, DynamicParameters parameters, IDbTransaction transaction = null)
        {
            if (transaction != null)
            {
                // Usar la transacción existente y la conexión asociada
                await transaction.Connection.ExecuteAsync(sqlQuery, parameters, transaction);
            }
            else
            {
                // No se pasó una transacción, manejar la conexión aquí
                using (OracleConnection connection = new OracleConnection(GetConnectionString()))
                {
                    await connection.OpenAsync();
                    await connection.ExecuteAsync(sqlQuery, parameters);
                }
            }
        }

        public IDbTransaction BeginTransaction(IDbConnection connection)
        {
            if (connection.State == ConnectionState.Open)
            {
                IDbTransaction transaction = connection.BeginTransaction();
                return transaction;
            }
            else
            {
                throw new InvalidOperationException("La conexión no está abierta.");
            }
        }

        public void CommitTransaction(IDbTransaction transaction)
        {
            if (transaction != null)
            {
                //transaction.Connection.Close();
                transaction.Commit();
            }
        }

        public void RollbackTransaction(IDbTransaction transaction)
        {
            if (transaction != null)
            {
                //transaction.Connection.Close();
                transaction.Rollback();

            }
        }

        public void CloseConnection(IDbConnection connection)
        {
            connection.Close();
        }
    }
}
