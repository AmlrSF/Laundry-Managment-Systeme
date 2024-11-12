using Laverie.Application.DTO;
using Laverie.Domain.Interface;
using Laverie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laverie.Application.Services
{
    public class LaverieService
    {
        private readonly ILaverieRepository _laverieRepository;

        public LaverieService(ILaverieRepository laverieRepository)
        {
            _laverieRepository = laverieRepository;
        }

        public async Task<LunadryDTO> AddLaverie(LunadryDTO laverieDto)
        {
            var laverie = new Laundry
            {
                Id = laverieDto.Id,
                NomLaverie = laverieDto.Name
            };

            await _laverieRepository.AddAsync(laverie);
            return laverieDto;
        }
        
       
    }
}
