using Laverie.Domain.Entities;
using Laverie.Domain.Interface;
using LaverieSystem.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laverie.Infrastructure.Repositories
{
    internal class CycleRepository : ICycleRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;

        public CycleRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Task AddAsync(Cycle cycle)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Cycle>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Cycle> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Cycle cycle)
        {
            throw new NotImplementedException();
        }
    }
}
