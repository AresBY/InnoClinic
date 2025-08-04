using InnoClinic.Authorization.Domain.Common.Enums;

using InnoClinicCommon.Enums;

namespace InnoClinic.Authorization.Domain.Entities;

public class Doctor : User
{
    public Doctor()
    {
        Role = UserRole.Doctor;
    }

    public Status WorkerStatus { get; set; }
}
