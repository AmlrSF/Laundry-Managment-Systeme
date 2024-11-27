using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laverie.Domain.Entities
{
    public class Laundry
    {
        public int id { get; set; }
        public string nomLaverie { get; set; }
        public List<Machine> machines { get; set; } = new List<Machine>();
    }
}
