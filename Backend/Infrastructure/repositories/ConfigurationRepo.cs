using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using Laverie.Domain.Entities;
using Laverie.API.Infrastructure.context; 
using Microsoft.Data.SqlClient;

public class ConfigurationRepo
{
    private readonly AppDbContext _dbContext;


    public ConfigurationRepo(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<User>> GetConfigAsync()
    {
        var users = new List<User>();


        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();

            string query = @"
                SELECT 
                    p.Id AS ProprietaireId, 
                    p.Name AS ProprietaireName, 
                    p.Email AS ProprietaireEmail, 
                    l.Id AS LaverieId, 
                    l.NomLaverie AS LaverieName, 
                    m.Id AS MachineId, 
                    m.Type AS MachineType, 
                    m.Status AS MachineStatus, 
                    c.Id AS CycleId, 
                   
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
                    p.Id, l.Id, m.Id";

            // Create the SQL command using the connection obtained from AppDbContext
            using (var command = new SqlCommand(query, (SqlConnection)connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                User currentProprietaire = null;

                while (await reader.ReadAsync())
                {
                    int proprietaireId = reader.GetInt32(0);

                    // Create or update the Proprietaire (User) object
                    if (currentProprietaire == null || currentProprietaire.id != proprietaireId)
                    {
                        currentProprietaire = new User
                        {
                            id = proprietaireId,
                            name = reader.GetString(1),
                            email = reader.GetString(2),
                            laundries = new List<Laundry>()
                        };
                        users.Add(currentProprietaire);
                    }

                    int? laverieId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3);
                    if (laverieId.HasValue)
                    {
                        var laverie = currentProprietaire.laundries
                            .Find(l => l.id == laverieId.Value) ?? new Laundry
                            {
                                id = laverieId.Value,
                                nomLaverie = reader.GetString(4),
                                machines = new List<Machine>()
                            };

                        if (!currentProprietaire.laundries.Contains(laverie))
                        {
                            currentProprietaire.laundries.Add(laverie);
                        }

                        int? machineId = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5);
                        if (machineId.HasValue)
                        {
                            var machine = laverie.machines
                                .Find(m => m.id == machineId.Value) ?? new Machine
                                {
                                    id = machineId.Value,
                                    type = reader.GetString(6),
                                    status = reader.GetBoolean(7),
                                    cycles = new List<Cycle>()
                                };

                            if (!laverie.machines.Contains(machine))
                            {
                                laverie.machines.Add(machine);
                            }


                        }
                    }
                }
            }
        }

        return users;
    }

    public async Task<bool> ToggleMachineAsync(int machineId)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();


            string getStatusQuery = @"
            SELECT Status 
            FROM Machine 
            WHERE Id = @MachineId";

            bool currentStatus = false;

            using (var getStatusCommand = new SqlCommand(getStatusQuery, (SqlConnection)connection))
            {
                getStatusCommand.Parameters.AddWithValue("@MachineId", machineId);

                var result = await getStatusCommand.ExecuteScalarAsync();
                if (result == null) return false;
                currentStatus = (bool)result;
            }

           
            string toggleStatusQuery = @"
            UPDATE Machine 
            SET Status = @NewStatus 
            WHERE Id = @MachineId";

            using (var toggleStatusCommand = new SqlCommand(toggleStatusQuery, (SqlConnection)connection))
            {
                toggleStatusCommand.Parameters.AddWithValue("@NewStatus", !currentStatus);
                toggleStatusCommand.Parameters.AddWithValue("@MachineId", machineId);

                int rowsAffected = await toggleStatusCommand.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }



    public async Task AddCycleAsync(Cycle cycle)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();

            // Insert the new cycle into the database without retrieving the ID
            string query = @"
            INSERT INTO Cycle (MachineId, Price, CycleDuration)
            VALUES (@MachineId, @Price , @CycleDuration);";

            using (var command = new SqlCommand(query, (SqlConnection)connection))
            {
                command.Parameters.AddWithValue("@MachineId", cycle.machineId);
                command.Parameters.AddWithValue("@Price", cycle.price);
                command.Parameters.AddWithValue("@CycleDuration", cycle.cycleDuration);

                await command.ExecuteNonQueryAsync(); // Simply execute the insertion
            }
        }
    }


}