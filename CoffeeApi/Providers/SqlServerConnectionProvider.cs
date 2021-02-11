using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Coffee.API.Providers
{
    public class SqlServerConnectionProvider : ISqlServerConnectionProvider
    {
        private readonly string connectionString;

        public SqlServerConnectionProvider(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnectionString");
        }

        public IDbConnection GetDbConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
