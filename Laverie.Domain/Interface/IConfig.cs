using Laverie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laverie.Domain.Interface
{
    public interface IConfig
    {
        Task<List<User>> GetConfig();
    }
}
