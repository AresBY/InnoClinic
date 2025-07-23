using InnoClinic.Application.Resources;
using InnoClinic.Server.Application.Exceptions;
using InnoClinic.Server.Application.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace InnoClinic.Application.Features.Auth.Queries
{
    public class CheckEmailExistsQueryHandler : IRequestHandler<CheckEmailExistsQuery, bool>
    {
        private readonly IUserRepository _userRepository;

        public CheckEmailExistsQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(CheckEmailExistsQuery request, CancellationToken cancellationToken)
        {
            bool exists = await _userRepository.ExistsAsync(request.Email, cancellationToken);
            if (exists)
            {
                throw new ConflictException(ErrorMessages.UserExist);
            }
            return false;
        }
    }
}
