using FluentValidation;

using InnoClinic.Services.Application.Resources;

public sealed class ChangeServiceStatusCommandValidator : AbstractValidator<ChangeServiceStatusCommand>
{
    public ChangeServiceStatusCommandValidator()
    {
        RuleFor(x => x.ServiceId).NotEmpty().WithMessage(ValidationMessages.ServiceId_Required);
    }
}
