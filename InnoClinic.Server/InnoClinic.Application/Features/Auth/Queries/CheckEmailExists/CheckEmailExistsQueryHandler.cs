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
        private readonly IPatientRepository _patientRepository;

        public CheckEmailExistsQueryHandler(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<bool> Handle(CheckEmailExistsQuery request, CancellationToken cancellationToken)
        {
            bool exists = await _patientRepository.ExistsAsync(request.Email, cancellationToken);
            if (exists)
            {
                throw new ConflictException(ErrorMessages.UserExist);
            }
            return false;
        }
    }
}
