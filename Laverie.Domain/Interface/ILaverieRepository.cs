using Laverie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laverie.Domain.Interface
{
    public interface ILaverieRepository
    {
        Task<Laundry> GetByIdAsync(int id);
        Task<IEnumerable<Laundry>> GetAllAsync();
        Task AddAsync(Laundry laundry);
        Task UpdateAsync(Laundry laundry);
        Task DeleteAsync(int id);
    }

}
