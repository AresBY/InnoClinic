using InnoClinic.Server.Application.Interfaces.Repositories;
using MediatR;

namespace InnoClinic.Server.Application.Features.Auth.Commands
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Unit>
    {
        private readonly IUserRepository _userRepository;

        public ConfirmEmailCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (user == null)
                throw new KeyNotFoundException("User not found.");

            if (!user.IsEmailConfirmed)
            {
                user.IsEmailConfirmed = true;
                await _userRepository.UpdateAsync(user, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
