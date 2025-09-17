using FluentValidation;

using InnoClinic.Services.Application.Features.Services.Commands.CreateService;
using InnoClinic.Services.Application.Interfaces.Repositories;
using InnoClinic.Services.Application.Resources;

public sealed class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
{
    public CreateServiceCommandValidator(ISpecializationRepository specializationRepository)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationMessages.ServiceName_Required);

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage(ValidationMessages.ServicePrice_Invalid);

        RuleFor(x => x.Category)
            .IsInEnum().WithMessage(ValidationMessages.ServiceCategory_Required);

        RuleFor(x => x.Status)
            .Must(x => x == true || x == false)
            .WithMessage(ValidationMessages.ServiceStatus_Invalid);

        RuleFor(x => x.SpecializationId)
            .Must(id =>
            {
                var specialization = specializationRepository.GetByIdAsync(id, CancellationToken.None)
                    .GetAwaiter()
                    .GetResult();
                return specialization != null;
            })
            .WithMessage(ValidationMessages.SpecializationIdInvalid);
    }
}
