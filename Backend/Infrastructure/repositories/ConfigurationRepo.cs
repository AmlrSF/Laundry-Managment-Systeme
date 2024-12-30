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
                c.Price AS CyclePrice, 
                c.CycleDuration AS CycleDuration,
                c.machineId As machineId
            FROM 
                Proprietaire p
            LEFT JOIN 
                Laverie l ON p.Id = l.ProprietaireId
            LEFT JOIN 
                Machine m ON l.Id = m.LaverieId
            LEFT JOIN 
                Cycle c ON m.Id = c.MachineId
            ORDER BY 
                p.Id, l.Id, m.Id, c.Id";

            using (var command = new SqlCommand(query, (SqlConnection)connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                User currentProprietaire = null;

                while (await reader.ReadAsync())
                {
                    int proprietaireId = reader.GetInt32(0);

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

                            int? cycleId = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8);
                            if (cycleId.HasValue)
                            {
                                var cycle = new Cycle
                                {
                                    id = cycleId.Value,
                                    price = reader.GetDecimal(9),
                                    cycleDuration = reader.GetString(10),
                                    machineId = reader.GetInt32(11)
                                };

                                if (!machine.cycles.Any(c => c.id == cycle.id))
                                {
                                    machine.cycles.Add(cycle);
                                }
                            }
                        }
                    }
                }
            }
        }

        return users;
    }

    public async Task<bool> StartMachine(Cycle cycle)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();

            string toggleStatusQuery = @"
        UPDATE Machine 
        SET Status = @NewStatus 
        WHERE Id = @MachineId";

            using (var toggleStatusCommand = new SqlCommand(toggleStatusQuery, (SqlConnection)connection))
            {
                toggleStatusCommand.Parameters.AddWithValue("@NewStatus", true);
                toggleStatusCommand.Parameters.AddWithValue("@MachineId", cycle.machineId);

                int rowsAffected = await toggleStatusCommand.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    // Calculate StartTime and EndTime
                    DateTime startTime = DateTime.Now;
                    int durationInMinutes = int.Parse(cycle.CycleDuration); // Convert duration from string to integer
                    DateTime endTime = startTime.AddMinutes(durationInMinutes);

                    // Create a new action for the cycle
                    string insertActionQuery = @"
                INSERT INTO Action (CycleId, StartTime, EndTime)
                VALUES (@CycleId, @StartTime, @EndTime)";

                    using (var insertActionCommand = new SqlCommand(insertActionQuery, (SqlConnection)connection))
                    {
                        insertActionCommand.Parameters.AddWithValue("@CycleId", cycle.id);  // CycleId from the passed cycle object
                        insertActionCommand.Parameters.AddWithValue("@StartTime", startTime);  // Current timestamp as StartTime
                        insertActionCommand.Parameters.AddWithValue("@EndTime", endTime);  // EndTime calculated above

                        int actionRowsAffected = await insertActionCommand.ExecuteNonQueryAsync();

                        // If the action insertion is successful, return true
                        return actionRowsAffected > 0;
                    }
                }
                else
                {
                    return false; // Machine status update failed
                }
            }
        }
    }

    public async Task<bool> StopMachine(Cycle cycle)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();

            string toggleStatusQuery = @"
        UPDATE Machine 
        SET Status = @NewStatus 
        WHERE Id = @MachineId";

            using (var toggleStatusCommand = new SqlCommand(toggleStatusQuery, (SqlConnection)connection))
            {
                toggleStatusCommand.Parameters.AddWithValue("@NewStatus", false);
                toggleStatusCommand.Parameters.AddWithValue("@MachineId", cycle.machineId);

                int rowsAffected = await toggleStatusCommand.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }

}