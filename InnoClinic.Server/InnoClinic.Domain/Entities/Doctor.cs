using InnoClinic.Domain.Common.Enums;

namespace InnoClinic.Server.Domain.Entities;

public class Doctor : User
{
    public Doctor()
    {
        Role = UserRole.Doctor;
    }

    public Status WorkerStatus { get; set; }
}
