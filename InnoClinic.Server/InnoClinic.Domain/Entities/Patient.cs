using InnoClinic.Domain.Common.Enums;

namespace InnoClinic.Server.Domain.Entities;

public class Patient : User
{
    public Patient()
    {
        Role = UserRole.Patient;
    }
}

