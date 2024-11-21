using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace Laverie.API.Infrastructure.context
{
    public class AppDbContext
    {
        private readonly string _connectionString;

        public AppDbContext(IConfiguration configuration)
        {
            
            _connectionString = configuration.GetConnectionString("SQLConnectionSB")
                                ?? throw new InvalidOperationException("Connection string 'SQLConnectionSB' is not configured.");
        }

        
        public IDbConnection CreateConnection()
        {
            var connection = new SqlConnection(_connectionString);
            return connection;
        }
    }
}
