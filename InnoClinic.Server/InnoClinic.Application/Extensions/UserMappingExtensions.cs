using InnoClinic.Server.Application.DTOs;
using InnoClinic.Server.Domain.Entities;

namespace InnoClinic.Server.Application.Extensions
{
    public static class UserMappingExtensions
    {
        public static UserDto ToDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                IsEmailConfirmed = user.IsEmailConfirmed,
                Role = user.Role,
            };
        }
    }
}
