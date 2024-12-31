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



        public async Task<bool> StartMachineStateAsync(int machineId, int idCycle)
        {
            try
            {
                // Construct the URL to call the backend API
                string url = $"api/Configuration/startMachine";

                // Prepare the request body
                var requestBody = new { MachineId = machineId, IdCycle = idCycle };

                // Send a POST request to start the machine
                var response = await _httpClient.PostAsJsonAsync(url, requestBody);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Machine started successfully.");
                    return true;
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode} - {errorResponse}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while starting the machine: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> StopMachineStateAsync(int machineId)
        {
            try
            {
                // Construct the URL to call the backend API
                string url = $"api/Configuration/stopMachine";

                // Send a POST request to stop the machine
                var response = await _httpClient.PostAsJsonAsync(url, machineId);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Machine stopped successfully.");
                    return true;
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode} - {errorResponse}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while stopping the machine: {ex.Message}");
                return false;
            }
        }


    }
}
