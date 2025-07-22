using FluentValidation;
using InnoClinic.Application.Resources;

namespace InnoClinic.Server.Application.Features.Auth.Commands;
public class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ValidationMessages.EmailIsRequired)
            .EmailAddress().WithMessage(ValidationMessages.InvalidEmail);

        RuleFor(x => x.Password)
           .NotEmpty().WithMessage(ValidationMessages.PasswordIsRequired)
           .Length(6, 15).WithMessage(ValidationMessages.PasswordLengthInvalid);
    }
}