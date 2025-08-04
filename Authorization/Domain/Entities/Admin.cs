using InnoClinicCommon.Enums;

namespace InnoClinic.Authorization.Domain.Entities;

public class Admin : User
{
    public Admin()
    {
        Role = UserRole.Admin;
    }
}
