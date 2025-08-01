using FluentValidation;

namespace InnoClinic.Offices.Application.Features.Office.Queries
{
    public class GetByIdQueryValidator : AbstractValidator<GetByIdQuery>
    {
        public GetByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id must be provided.");
        }
    }
}