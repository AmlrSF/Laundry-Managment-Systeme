using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laverie.Application.DTO
{
    public class LunadryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public LunadryDTO(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
