using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Laverie.Domain.Entities;
using System.Net.Http.Json;
using System.Transactions;

namespace Laverie.SimulationApp.Services
{
    public class LaundryService
    {
        private readonly HttpClient _httpClient;

        
        public LaundryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

       
        public async Task<List<User>> GetConfigurationAsync()
        {
            try
            {
               
                var response = await _httpClient.GetAsync("api/Configuration");

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();

                   
                    var users = JsonSerializer.Deserialize<List<User>>(responseBody);

                    return users ?? new List<User>(); 
                }
                else
                {
                   
                    throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
              
                Console.WriteLine($"An error occurred while fetching data: {ex.Message}");
                return new List<User>(); 
            }
        }


        public async Task<bool> AddCycleAsync(int machineId, decimal price, int cycleDuration)
        {
        
            bool machineToggled = await ToggleMachineStateAsync(machineId);

            if (!machineToggled)
            {
                Console.WriteLine("Failed to toggle the machine state.");
                return false;
            }

            var cycleData = new
            {
                machineId = machineId,
                price = price,
                cycleDuration = cycleDuration,
                transactions = new List<object>()
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Configuration/addCycle", cycleData);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Cycle added successfully!");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding the cycle: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> ToggleMachineStateAsync(int machineId)
        {
            try
            {
                // Construct the URL to call the backend API (ensure it matches your backend route)
                string url = $"api/Configuration/toggle-machine/{machineId}";

                // Send a PUT request to toggle the machine's state
                var response = await _httpClient.PutAsJsonAsync(url, new { machineId = machineId });

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Machine state toggled successfully.");
                    return true;
                }
                else
                {
                    // Handle error and provide useful feedback
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode} - {errorResponse}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while toggling the machine state: {ex.Message}");
                return false;
            }
        }

    }
}
