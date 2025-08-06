using InnoClinic.Profiles.Application.Features.Doctor.Commands.CreateDoctorProfile;

using InnoClinicCommon.Enums;

using Swashbuckle.AspNetCore.Filters;

namespace InnoClinic.Offices.Application.Features.Doctor.Commands.Examples
{
    public class CreateDoctorCommandExample : IExamplesProvider<CreateDoctorProfileCommand>
    {
        public CreateDoctorProfileCommand GetExamples()
        {
            return new CreateDoctorProfileCommand
            {
                FirstName = "Anna",
                LastName = "Petrova",
                MiddleName = "Sergeevna",
                DateOfBirth = new DateTime(1980, 5, 10),
                Email = "anna.petrova@clinic.com",
                Specialization = "Therapist",
                OfficeId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                CareerStartYear = 2005,
                Status = DoctorStatus.AtWork
            };
        }
    }
}
