using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Laverie.Domain.Entities
{
    public class Cycle
    {
        public int id { get; set; }
        public decimal price { get; set; }
        public int machineId { get; set; }
        public string cycleDuration { get; set; }
        public List<Action> transactions { get; set; }
    }

}
