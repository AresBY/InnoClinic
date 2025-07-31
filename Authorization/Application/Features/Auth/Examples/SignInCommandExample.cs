using Swashbuckle.AspNetCore.Filters;
using InnoClinic.Authorization.Application.Features.Auth.Commands;
using InnoClinicCommon.Enums;

namespace Application.Features.Auth.Examples
{
    public class SignInCommandExample : IExamplesProvider<SignInCommand>
    {
        public SignInCommand GetExamples()
        {
            return new SignInCommand
            {
                //Email = "Admin@example.com",
                //Password = "adminadmin",
                //Role = UserRole.Admin
                Email = "free21@mail.ru",
                Password = "free21",
                Role = UserRole.Receptionist
            };
        }
    }
}
