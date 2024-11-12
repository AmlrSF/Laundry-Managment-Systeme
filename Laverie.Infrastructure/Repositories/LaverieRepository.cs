using Laverie.Domain.Entities;
using Laverie.Domain.Interface;
using LaverieSystem.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laverie.Infrastructure.Repositories
{
    public class LaverieRepository : ILaverieRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;

        public LaverieRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Task AddAsync(Laundry laundry)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Laundry>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Laundry> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Laundry laundry)
        {
            throw new NotImplementedException();
        }
    }
}