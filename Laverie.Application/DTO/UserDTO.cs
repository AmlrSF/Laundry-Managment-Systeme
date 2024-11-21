using Laverie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laverie.Application.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }
        public string Pass { get; set; }


        public List<Laundry> Laundries { get; set; } = new List<Laundry>();

        public UserDTO(int id, string name, string email , string pass, List<Laundry> l )
        {
            Id = id;
            Name = name;
            Email = email;
            Laundries = l;
            Pass = pass;
        }
    }
}
