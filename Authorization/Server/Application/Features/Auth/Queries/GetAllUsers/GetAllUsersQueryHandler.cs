using InnoClinic.Authorization.Application.DTOs;
using InnoClinic.Authorization.Application.Extensions;
using InnoClinic.Authorization.Application.Features.Auth.Queries;
using InnoClinic.Authorization.Application.Interfaces.Repositories;
using MediatR;

namespace InnoClinic.Authorization.Application.Features.Auth.Queries;

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
