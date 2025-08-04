using FluentValidation;

namespace InnoClinic.Offices.Application.Features.Office.Queries.GetOffice
{
    public class GetOfficeByIdQueryValidator : AbstractValidator<GetOfficeByIdQuery>
    {
        public GetOfficeByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id must be provided.");
        }
    }
}