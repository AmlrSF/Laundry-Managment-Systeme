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
        public int Age { get; set; }

        public string Password { get; set; }
        public string Email { get; set; }

        public UserDTO(int id, string name, int age, string email, string Pass)
        {
            Id = id;
            Name = name;
            Age = age;
            Email = email;
            Password = Pass;
        }
    }
}
