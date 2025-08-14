using FluentValidation;

namespace InnoClinic.Profiles.Application.Features.Patient.Commands.CreatePatientProfile
{
    public sealed class CreatePatientProfileByOwnCommandValidator : AbstractValidator<CreatePatientProfileByOwnCommand>
    {
        public CreatePatientProfileByOwnCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Please, enter the first name");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Please, enter the last name");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Please, enter the phone number")
                .Matches(@"^\+\d+$").WithMessage("You've entered an invalid phone number");

            RuleFor(x => x.DateOfBirth)
                .NotEqual(default(DateOnly)).WithMessage("Please, select the date");
        }
    }
}
