using Laverie.Infrastructure.Repositories;
using LaverieSystem.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LaverieConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Set up configuration (you may need to adjust the path to your appsettings.json)
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Adjust the path if necessary
                .AddJsonFile("appsettings.json")
                .Build();

            // Create the connection factory and repository
            var connectionFactory = new SqlServerConnectionFactory(configuration);
            var repository = new ConfigurationRepository(connectionFactory);

            // Create tables
            await repository.CreateTablesAsync();

            // Insert initial data
            await repository.InsertInitialDataAsync();

            Console.WriteLine("Tables created and initial data inserted successfully.");
        }
    }
}
