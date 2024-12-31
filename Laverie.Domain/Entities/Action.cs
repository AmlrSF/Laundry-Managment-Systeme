using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laverie.Domain.Entities
{
    public class Action
    {
        public int Id { get; set; } // Primary key
        public int CycleId { get; set; } // Foreign key to Cycle
        public DateTime StartTime { get; set; } // When the cycle starts
        
    }
}
