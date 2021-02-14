using System.Data;
using Dapper.AmbientContext;
using MySql.Data.MySqlClient;

namespace AccountManager.Api.Connection
{
    public class MySqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public MySqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection Create()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}