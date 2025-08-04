using FluentValidation;

namespace InnoClinic.Offices.Application.Features.Office.Commands.UpdateOffice;

public class UpdateOfficeCommandValidator : AbstractValidator<UpdateOfficeCommand>
{
    public UpdateOfficeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.City).NotEmpty().WithMessage("Please, enter the office’s city");
        RuleFor(x => x.Street).NotEmpty().WithMessage("Please, enter the office’s street");
        RuleFor(x => x.HouseNumber).NotEmpty().WithMessage("Please, enter the office’s house number");

        RuleFor(x => x.RegistryPhoneNumber)
                 .NotEmpty().WithMessage("Please, enter the phone number")
                 .Matches(@"^\+?\d+$").WithMessage("You've entered an invalid phone number");
    }
}
