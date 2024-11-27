using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Laverie.SimulationApp.Services;

namespace LaverieConsoleApp
{
    class Program
    {
        static LaundryService _laundryService;

        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7244/") };
            _laundryService = new LaundryService(httpClient);

            while (true)
            {
                try
                {
                    await ShowMainMenu();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        static async Task ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Laundry Management System ===");
            Console.WriteLine("1. View Configuration");
            Console.WriteLine("0. Exit");

            Console.Write("\nEnter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ViewConfiguration();
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }

        static async Task ViewConfiguration()
        {
            Console.Clear();
            Console.WriteLine("=== View Configuration ===");

            try
            {
                // Fetch the configuration data
                var owners = await _laundryService.GetConfigurationAsync();

                // Check if there is any data
                if (owners.Count == 0)
                {
                    Console.WriteLine("No data found.");
                    return;
                }

                // Display the list of owners
                Console.WriteLine("List of Users:");
                for (int i = 0; i < owners.Count; i++)
                {
                    var owner = owners[i];
                    Console.WriteLine($"{i + 1}. {owner.name ?? "Unknown"} (Email: {owner.email ?? "No Email"})");
                }

                Console.Write("\nSelect a user by number: ");
                int selectedUserIndex = int.Parse(Console.ReadLine()) - 1;

                // Validate the selection
                if (selectedUserIndex < 0 || selectedUserIndex >= owners.Count)
                {
                    Console.WriteLine("Invalid selection. Returning to main menu...");
                    Console.ReadKey();
                    return;
                }

                var selectedOwner = owners[selectedUserIndex];

                Console.WriteLine($"\nYou selected: {selectedOwner.name ?? "Unknown"} (Email: {selectedOwner.email ?? "No Email"})");

                // Check if the selected user has laundries
                if (selectedOwner.laundries?.Count > 0)
                {
                    // Display the laundries of the selected user
                    Console.WriteLine("\nLaundries:");
                    for (int i = 0; i < selectedOwner.laundries.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {selectedOwner.laundries[i].nomLaverie ?? "Unnamed"}");
                    }

                    Console.Write("\nSelect a laundry by number: ");
                    int selectedLaundryIndex = int.Parse(Console.ReadLine()) - 1;

                    // Validate the selection
                    if (selectedLaundryIndex < 0 || selectedLaundryIndex >= selectedOwner.laundries.Count)
                    {
                        Console.WriteLine("Invalid selection. Returning to main menu...");
                        Console.ReadKey();
                        return;
                    }

                    var selectedLaundry = selectedOwner.laundries[selectedLaundryIndex];

                    Console.WriteLine($"\nYou selected: {selectedLaundry.nomLaverie ?? "Unnamed"}");

                    if (selectedLaundry.machines?.Count > 0)
                    {
                        Console.WriteLine("\nMachines:");
                        for (int i = 0; i < selectedLaundry.machines.Count; i++)
                        {
                            var machine = selectedLaundry.machines[i];
                            Console.WriteLine($"{i + 1}. Machine {machine.id}: {machine.type ?? "Unknown"} - Status: {(machine.status ? "In Use" : "Available")}");
                        }

                        Console.Write("\nSelect a machine by number: ");
                        int selectedMachineIndex = int.Parse(Console.ReadLine()) - 1;

                        // Validate the selection
                        if (selectedMachineIndex < 0 || selectedMachineIndex >= selectedLaundry.machines.Count)
                        {
                            Console.WriteLine("Invalid selection. Returning to main menu...");
                            Console.ReadKey();
                            return;
                        }

                        var selectedMachine = selectedLaundry.machines[selectedMachineIndex];

                        Console.WriteLine($"\nYou selected Machine {selectedMachine.id}: {selectedMachine.type ?? "Unknown"}");

                        Console.WriteLine("\nEnter the cycle details:");

                        Console.Write("Price: ");
                        decimal cyclePrice = decimal.Parse(Console.ReadLine());

                        Console.Write("Duration (in minutes): ");
                        int cycleDuration = int.Parse(Console.ReadLine());

                        // Send the cycle data to the server
                        var success = await _laundryService.AddCycleAsync(selectedMachine.id, cyclePrice, cycleDuration);

                        if (success)
                        {
                            Console.WriteLine($"Cycle added successfully! Machine {selectedMachine.id} is now working with cycle details (Price: {cyclePrice}, Duration: {cycleDuration}).");
                        }
                        else
                        {
                            Console.WriteLine("Failed to add cycle.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No machines available in this laundry.");
                    }
                }
                else
                {
                    Console.WriteLine("No laundries available.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching data: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }


    }
}
