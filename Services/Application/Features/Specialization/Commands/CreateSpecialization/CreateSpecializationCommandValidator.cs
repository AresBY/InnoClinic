using FluentValidation;

using InnoClinic.Services.Application.Features.Specialization.Commands.CreateSpecialization;
using InnoClinic.Services.Application.Resources;

namespace InnoClinic.Specializations.Application.Features.Specializations.Commands.CreateSpecialization
{
    public sealed class CreateSpecializationCommandValidator : AbstractValidator<CreateSpecializationCommand>
    {
        public CreateSpecializationCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(ValidationMessages.SpecializationNameRequired);

            RuleFor(x => x.Services)
                .NotEmpty()
                .WithMessage(ValidationMessages.ServicesRequired);

            RuleForEach(x => x.Services).ChildRules(service =>
            {
                service.RuleFor(s => s.Name)
                    .NotEmpty()
                    .WithMessage(ValidationMessages.ServiceNameRequired);

                service.RuleFor(s => s.Price)
                    .GreaterThan(0)
                    .WithMessage(ValidationMessages.ServicePriceInvalid);

                service.RuleFor(s => s.Category)
                    .IsInEnum()
                    .WithMessage(ValidationMessages.ServiceCategoryInvalid);
            });
        }
    }
}
