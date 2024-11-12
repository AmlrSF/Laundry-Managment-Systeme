using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LaverieSystem.Infrastructure.Data
{
    public class SqlServerConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public SqlServerConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection CreateConnection()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            return new SqlConnection(connectionString);
        }
    }
}
