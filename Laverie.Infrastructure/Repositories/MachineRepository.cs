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
    public class MachineRepository : IMachineRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;

        public MachineRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Task AddAsync(Machine machine)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Machine>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Machine> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Machine machine)
        {
            throw new NotImplementedException();
        }
    }
}
