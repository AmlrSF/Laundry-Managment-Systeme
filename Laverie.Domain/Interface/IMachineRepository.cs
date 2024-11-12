using Laverie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laverie.Domain.Interface
{
    public interface IMachineRepository
    {
        Task<Machine> GetByIdAsync(int id);
        Task<IEnumerable<Machine>> GetAllAsync();
        Task AddAsync(Machine machine);
        Task UpdateAsync(Machine machine);
        Task DeleteAsync(int id);
    }

}