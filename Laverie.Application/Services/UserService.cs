using Laverie.Application.DTO;
using Laverie.Domain.Entities;
using Laverie.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laverie.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDTO> RegisterUser(UserDTO userDto)
        {
            
            var user = new User
            {

                Name = userDto.Name,
                Password = userDto.Pass,
                Email = userDto.Email
            };

            await _userRepository.AddAsync(user);
            return userDto;
        }

     
    }
}
