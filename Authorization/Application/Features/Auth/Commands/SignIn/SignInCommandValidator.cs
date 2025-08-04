using FluentValidation;

using InnoClinic.Authorization.Application.Resources;

namespace InnoClinic.Authorization.Application.Features.Auth.Commands.SignIn;
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