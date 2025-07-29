using InnoClinicCommon.Enums;

namespace InnoClinic.Authorization.Domain.Entities;

public class Patient : User
{
    public Patient()
    {
        Role = UserRole.Patient;
    }
}

