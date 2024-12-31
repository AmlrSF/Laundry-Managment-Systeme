using Azure.Core;
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

        public async Task<bool> starteMachineAsync(int MachineId, int IdCycle)
        {
            return await _configurationRepo.StartMachine(MachineId, IdCycle);
        }
        public async Task<bool> stopeMachineAsync(int MachineId)
        {
            return await _configurationRepo.StopMachine(MachineId);
        }




    }
}
