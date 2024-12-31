  using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laverie.Domain.DTOS
{
    public class StartMachineRequest
    {
        public int MachineId { get; set; }
        public int IdCycle { get; set; }

    }
}
