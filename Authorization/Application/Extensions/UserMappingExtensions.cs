using InnoClinic.Authorization.Application.DTOs;
using InnoClinic.Authorization.Domain.Entities;

namespace InnoClinic.Authorization.Application.Extensions
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
