﻿using Laverie.Domain.Entities;
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
    }
}