using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laverie.Domain.Entities
{
    public class Machine
    {
        public int id { get; set; }
        
        public bool status { get; set; }
        public string type { get; set; }
        public List<Cycle> cycles { get; set; } = new List<Cycle>();
    }
}
