using InnoClinic.Server.Application.DTOs;
using MediatR;

namespace InnoClinic.Server.Application.Features.Auth.Queries;

public class GetAllUsersQuery : IRequest<List<UserDto>>
{
}
