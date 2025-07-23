using FluentValidation;
using InnoClinic.Application.Resources;

namespace InnoClinic.Server.Application.Features.Auth.Commands;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ValidationMessages.EmailIsRequired)
            .EmailAddress().WithMessage(ValidationMessages.InvalidEmail);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ValidationMessages.PasswordIsRequired)
            .Length(6, 15).WithMessage(ValidationMessages.PasswordLengthInvalid); 
   
        RuleFor(x => x.ReEnteredPassword)
            .NotEmpty().WithMessage(ValidationMessages.PasswordIsRequired)
            .Equal(x => x.Password).WithMessage(ValidationMessages.PasswordsDoNotMatch);
    }
}
