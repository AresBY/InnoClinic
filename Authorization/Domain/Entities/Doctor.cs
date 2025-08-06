using InnoClinicCommon.Enums;

namespace InnoClinic.Authorization.Domain.Entities;

public class Doctor : User
{
    public Doctor()
    {
        Role = UserRole.Doctor;
    }

    public DoctorStatus WorkerStatus { get; set; }
}
