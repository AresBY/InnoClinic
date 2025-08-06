using FluentValidation;

namespace InnoClinic.Profiles.Application.Features.Doctor.Commands.CreateDoctorProfile
{
    public class CreateDoctorProfileCommandValidator : AbstractValidator<CreateDoctorProfileCommand>
    {
        public CreateDoctorProfileCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Please, enter the first name");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Please, enter the last name");

            RuleFor(x => x.DateOfBirth)
                .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("Date of birth must be today or earlier");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Please, enter the email")
                .EmailAddress().WithMessage("You've entered an invalid email");

            RuleFor(x => x.Specialization)
                .NotEmpty().WithMessage("Please, choose the specialisation");

            RuleFor(x => x.OfficeId)
                .NotEqual(Guid.Empty).WithMessage("Please, choose the office");

            RuleFor(x => x.CareerStartYear)
                .InclusiveBetween(1900, DateTime.Today.Year)
                .WithMessage("Please, select a valid career start year");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid status value");
        }
    }
}
