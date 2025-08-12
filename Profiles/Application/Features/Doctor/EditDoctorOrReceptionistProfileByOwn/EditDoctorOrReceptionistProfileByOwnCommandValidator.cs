using FluentValidation;

namespace InnoClinic.Profiles.Application.Features.Doctor.Commands.EditDoctorProfile
{
    public class EditDoctorOrReceptionistProfileByOwnCommandValidator : AbstractValidator<EditDoctorOrReceptionistProfileByOwnCommand>
    {
        public EditDoctorOrReceptionistProfileByOwnCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Please, enter the first name");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Please, enter the last name");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Please, select the date")
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Date of birth cannot be in the future");

            RuleFor(x => x.Specialization)
                .NotEmpty().WithMessage("Please, choose the specialisation");

            RuleFor(x => x.CareerStartYear)
                .InclusiveBetween(1900, DateTime.Today.Year)
                .WithMessage("Please, select the year");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid status");

        }
    }
}
