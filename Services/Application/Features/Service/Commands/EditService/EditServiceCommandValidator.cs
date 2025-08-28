using FluentValidation;

using InnoClinic.Services.Application.Features.Services.Commands.EditService;
using InnoClinic.Services.Application.Resources;

public sealed class EditServiceCommandValidator : AbstractValidator<EditServiceCommand>
{
    public EditServiceCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationMessages.ServiceName_Required);

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage(ValidationMessages.ServicePrice_Invalid);

        RuleFor(x => x.Category)
            .IsInEnum().WithMessage(ValidationMessages.ServiceCategory_Required);
    }
}
