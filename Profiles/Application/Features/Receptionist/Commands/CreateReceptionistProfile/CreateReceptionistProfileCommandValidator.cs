using FluentValidation;

namespace InnoClinic.Profiles.Application.Features.Receptionist.Commands.CreateReceptionistProfile
{
    public sealed class CreateReceptionistProfileCommandValidator : AbstractValidator<CreateReceptionistProfileCommand>
    {
        public CreateReceptionistProfileCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Please, enter the first name.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Please, enter the last name.");

            RuleFor(x => x.OwnerId)
                .NotEmpty().WithMessage("OwnerId is required.");

            RuleFor(x => x.OfficeId)
                .NotEmpty().WithMessage("Please, choose the office.");
        }
    }
}
