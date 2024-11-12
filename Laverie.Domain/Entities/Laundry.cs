using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laverie.Domain.Entities
{
    public class Laundry
    {
        public int Id { get; set; }
        public string NomLaverie { get; set; }
        public List<Machine> Machines { get; set; } = new List<Machine>();
    }
}
