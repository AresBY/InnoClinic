using InnoClinic.Authorization.Application.Features.Auth.Commands.SignIn;

using InnoClinicCommon.Enums;

using Swashbuckle.AspNetCore.Filters;

namespace Application.Features.Auth.Examples
{
    public class SignInCommandExample : IExamplesProvider<SignInCommand>
    {
        public SignInCommand GetExamples()
        {
            return new SignInCommand
            {
                //Email = "Receptionist@mail.ru",
                //Password = "Receptionist",
                //Role = UserRole.Receptionist
                Email = "Patient@mail.ru",
                Password = "Patient",
                Role = UserRole.Patient
            };
        }
    }
}
