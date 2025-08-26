using FluentValidation;

namespace InnoClinic.Services.Application.Features.Services.Commands.CreateService
{
    public sealed class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
    {
        public CreateServiceCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Please, enter the name");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("You've entered an invalid price");

            RuleFor(x => x.Category)
                .IsInEnum().WithMessage("Please, choose a valid service category");

            RuleFor(x => x.Status)
                .Must(x => x == true || x == false)
                .WithMessage("Status must be true (Active) or false (Inactive)");
        }
    }
}
