using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Profiles.Application.Features.Doctor.Commands.CreateDoctorProfile;
using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.Application.Mappings;

public static class ProfileMappingExtensions
{
    public static DoctorProfile ToDoctorProfileEntity(this CreateDoctorProfileCommand request)
    {
        return new DoctorProfile
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            MiddleName = request.MiddleName,
            DateOfBirth = request.DateOfBirth,
            Email = request.Email,
            Specialization = request.Specialization,
            OfficeId = request.OfficeId,
            CareerStartYear = request.CareerStartYear,
            Status = request.Status
        };
    }

    public static DoctorProfileDto ToDto(this DoctorProfile entity)
    {
        return new DoctorProfileDto
        {
            Id = entity.ID,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            MiddleName = entity.MiddleName,
            DateOfBirth = entity.DateOfBirth,
            Email = entity.Email,
            Specialization = entity.Specialization,
            OfficeId = entity.OfficeId,
            CareerStartYear = entity.CareerStartYear,
            Status = entity.Status
        };
    }
}
