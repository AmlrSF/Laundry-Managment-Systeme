using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using Laverie.Domain.Entities;
using Laverie.API.Infrastructure.context; // AppDbContext namespace
using Microsoft.Data.SqlClient;

public class ConfigurationRepo
{
    private readonly AppDbContext _dbContext;

    // Inject AppDbContext to get access to the connection string and connection management
    public ConfigurationRepo(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<User>> GetConfigAsync()
    {
        var users = new List<User>();

        // Use the CreateConnection method from AppDbContext to get the connection
        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();

            string query = @"
                SELECT 
                    p.Id AS ProprietaireId, 
                    p.Name AS ProprietaireName, 
                    l.Id AS LaverieId, 
                    l.NomLaverie AS LaverieName, 
                    m.Id AS MachineId, 
                    m.Type AS MachineType, 
                    c.Id AS CycleId, 
                    c.Date AS CycleDate, 
                    c.Price AS CyclePrice,
                    p.Email As ProprietaireEmail
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

            // Create the SQL command using the connection obtained from AppDbContext
            using (var command = new SqlCommand(query, (SqlConnection)connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                User currentProprietaire = null;

                while (await reader.ReadAsync())
                {
                    int proprietaireId = reader.GetInt32(0);

                    // Create or update the Proprietaire (User) object
                    if (currentProprietaire == null || currentProprietaire.Id != proprietaireId)
                    {
                        currentProprietaire = new User
                        {
                            Id = proprietaireId,
                            Name = reader.GetString(1),
                            Email = reader.GetString(9),
                            Laundries = new List<Laundry>()
                        };
                        users.Add(currentProprietaire);
                    }

                    int? laverieId = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2);
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

                        int? machineId = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4);
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

        return users;
    }
}
