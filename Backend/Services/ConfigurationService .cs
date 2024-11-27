using Laverie.Domain.Entities;
using Laverie.Domain.Interface;

namespace Laverie.API.Services
{
    public class ConfigurationService
    {
        private readonly ConfigurationRepo _configurationRepo;

        public ConfigurationService(ConfigurationRepo configurationRepo)
        {
            _configurationRepo = configurationRepo;
        }

        public async Task<List<User>> GetConfig()
        {
            return await _configurationRepo.GetConfigAsync();
        }

        public async Task<bool> ToggleMachineAsync(int machineId)
        {
            return await _configurationRepo.ToggleMachineAsync(machineId);
        }

        public async Task AddCycleAsync(Cycle cycle)
        {
            await _configurationRepo.AddCycleAsync(cycle);
        }


    }
}
