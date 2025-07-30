using InnoClinicCommon.Enums;

namespace InnoClinic.Authorization.Domain.Entities;
public class Receptionist : User
{
    public Receptionist()
    {
        Role = UserRole.Receptionist;
    }
}

