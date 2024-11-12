using Laverie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laverie.Domain.Interface
{
    public interface ICycleRepository
    {
        Task<Cycle> GetByIdAsync(int id);
        Task<IEnumerable<Cycle>> GetAllAsync();
        Task AddAsync(Cycle cycle);
        Task UpdateAsync(Cycle cycle);
        Task DeleteAsync(int id);
    }

}
