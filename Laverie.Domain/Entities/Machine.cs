using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laverie.Domain.Entities
{
    public class Machine
    {
        public int Id { get; set; }
        public string LaverieName { get; set; }
        public string Type { get; set; }
        public List<Cycle> Cycles { get; set; } = new List<Cycle>();
    }
}
