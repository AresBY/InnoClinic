using InnoClinic.Profiles.Application.Features.Patient.Commands.CreatePatientProfile;

using Swashbuckle.AspNetCore.Filters;

namespace InnoClinic.Profiles.Application.Features.Patient.Examples
{
    public class CreatePatientProfileCommandExample : IExamplesProvider<CreatePatientProfileCommand>
    {
        public CreatePatientProfileCommand GetExamples()
        {
            return new CreatePatientProfileCommand
            {
                FirstName = "John",
                LastName = "Doe",
                MiddleName = "Michael",
                PhoneNumber = "+1234567890",
                DateOfBirth = new DateOnly(1985, 7, 20)
            };
        }
    }
}
