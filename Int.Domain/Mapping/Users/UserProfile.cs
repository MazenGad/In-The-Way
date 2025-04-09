using AutoMapper;
using Int.Domain.DTOs.Users;
using Int.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Int.Domain.Mapping.Cars
{
	public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>(); // تحويل Userr إلى UserDTO
        }
    }
}
