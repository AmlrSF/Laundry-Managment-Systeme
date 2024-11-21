using Laverie.Domain.Entities;
using Laverie.Domain.Interface;
using LaverieSystem.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Laverie.Infrastructure.Repositories
{
    public class ConfigurationRepository : IConfig
    {
        private readonly SqlServerConnectionFactory _connectionFactory;

        public ConfigurationRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<User>> GetConfig()
        {
            var configurations = new List<User>();

            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                string query = @"
                    SELECT 
                        p.Id AS ProprietaireId, 
                        p.Name AS ProprietaireName, 
                        l.Id AS LaverieId, 
                        l.Name AS LaverieName, 
                        m.Id AS MachineId, 
                        m.Type AS MachineType, 
                        c.Id AS CycleId, 
                        c.Date AS CycleDate, 
                        c.Price AS CyclePrice
                    FROM 
                        Proprietaire p
                    LEFT JOIN 
                        Laverie l ON p.Id = l.ProprietaireId
                    LEFT JOIN 
                        Machine m ON l.Id = m.LaverieId
                    LEFT JOIN 
                        Cycle c ON m.Id = c.MachineId
                    ORDER BY 
                        p.Id, l.Id, m.Id, c.Date";

                var command = new SqlCommand(query, connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    User currentProprietaire = null;

                    while (await reader.ReadAsync())
                    {
                        int proprietaireId = reader.GetInt32(0);

                        if (currentProprietaire == null || currentProprietaire.Id != proprietaireId)
                        {
                            currentProprietaire = new User
                            {
                                Name = reader.GetString(1),
                                Password = reader.GetString(2),
                                Email = reader.GetString(3),
                                Age = reader.GetInt32(4),
                                Laundries = new List<Laundry>()
                            };
                            configurations.Add(currentProprietaire);
                        }

                        int? laverieId = reader.IsDBNull(2) ? null : (int?)reader.GetInt32(2);
                        if (laverieId.HasValue)
                        {
                            var laverie = currentProprietaire.Laundries
                                .Find(l => l.Id == laverieId.Value) ?? new Laundry
                                {
                                    Id = laverieId.Value,
                                    NomLaverie = reader.GetString(3),
                                    Machines = new List<Machine>()
                                };

                            if (!currentProprietaire.Laundries.Contains(laverie))
                            {
                                currentProprietaire.Laundries.Add(laverie);
                            }

                            int? machineId = reader.IsDBNull(4) ? null : (int?)reader.GetInt32(4);
                            if (machineId.HasValue)
                            {
                                var machine = laverie.Machines
                                    .Find(m => m.Id == machineId.Value) ?? new Machine
                                    {
                                        Id = machineId.Value,
                                        Type = reader.GetString(5),
                                        Cycles = new List<Cycle>()
                                    };

                                if (!laverie.Machines.Contains(machine))
                                {
                                    laverie.Machines.Add(machine);
                                }

                                if (!reader.IsDBNull(6))
                                {
                                    var cycle = new Cycle
                                    {
                                        Id = reader.GetInt32(6),
                                        Date = reader.GetDateTime(7),
                                        Price = reader.GetDecimal(8)
                                    };
                                    machine.Cycles.Add(cycle);
                                }
                            }
                        }
                    }
                }
            }

            return configurations;
        }

        
        public async Task CreateTablesAsync()
        {
            var createTablesQuery = @"
                CREATE TABLE Proprietaire (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Name NVARCHAR(100) NOT NULL,
                    Email NVARCHAR(100) NOT NULL UNIQUE,
                    Password NVARCHAR(100) NOT NULL,
                    Age INT NOT NULL
                );

                CREATE TABLE Laverie (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    NomLaverie NVARCHAR(100) NOT NULL,
                    ProprietaireId INT,
                    FOREIGN KEY (ProprietaireId) REFERENCES Proprietaire(Id) ON DELETE CASCADE
                );

                CREATE TABLE Machine (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Type NVARCHAR(50) NOT NULL,
                    LaverieId INT,
                    FOREIGN KEY (LaverieId) REFERENCES Laverie(Id) ON DELETE CASCADE
                );

                CREATE TABLE Cycle (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Date DATETIME NOT NULL,
                    Price DECIMAL(10,2) NOT NULL,
                    MachineId INT,
                    FOREIGN KEY (MachineId) REFERENCES Machine(Id) ON DELETE CASCADE
                );
            ";

            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand(createTablesQuery, connection);
                await command.ExecuteNonQueryAsync();
            }

            Console.WriteLine("Tables created successfully.");
        }

       
        
    }
}
