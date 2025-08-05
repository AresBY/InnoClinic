using InnoClinic.Authorization.Application.Interfaces.Repositories;
using InnoClinic.Authorization.Application.Resources;

using InnoClinicCommon.Exception;

using MediatR;

namespace InnoClinic.Authorization.Application.Features.Auth.Queries.CheckEmailExists
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
