using InnoClinic.Authorization.Application.DTOs;
using MediatR;

namespace InnoClinic.Authorization.Application.Features.Auth.Queries;

public class GetAllUsersQuery : IRequest<List<UserDto>>
{
}
