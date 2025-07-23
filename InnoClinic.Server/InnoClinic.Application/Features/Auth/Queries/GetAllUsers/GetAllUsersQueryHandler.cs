using InnoClinic.Server.Application.DTOs;
using InnoClinic.Server.Application.Extensions;
using InnoClinic.Server.Application.Features.Auth.Queries;
using InnoClinic.Server.Application.Interfaces.Repositories;
using MediatR;

namespace InnoClinic.Application.Features.Auth.Queries;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        return users.Select(p => p.ToDto()).ToList();
    }
}
