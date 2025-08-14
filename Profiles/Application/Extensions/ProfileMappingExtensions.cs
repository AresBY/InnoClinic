using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Profiles.Application.DTOs;
using InnoClinic.Profiles.Application.Features.Doctor.Commands.CreateDoctorProfile;
using InnoClinic.Profiles.Application.Features.Patient.Commands.CreatePatientProfile;
using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.Application.Mappings
{
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
                Status = request.Status,
                OwnerId = request.OwnerId
            };
        }

        public static DoctorProfileDto ToDto(this DoctorProfile entity)
        {
            return new DoctorProfileDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                MiddleName = entity.MiddleName,
                DateOfBirth = entity.DateOfBirth,
                Email = entity.Email,
                Specialization = entity.Specialization,
                OfficeId = entity.OfficeId,
                CareerStartYear = entity.CareerStartYear,
                Status = entity.Status,
                OwnerId = entity.OwnerId
            };
        }

        public static PatientProfileDto ToDto(this PatientProfile entity)
        {
            return new PatientProfileDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                MiddleName = entity.MiddleName,
                PhoneNumber = entity.PhoneNumber,
                DateOfBirth = entity.DateOfBirth
            };
        }

        public static PatientProfile ToPatientProfileEntity(this CreatePatientProfileCommand request)
        {
            return new PatientProfile
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                PhoneNumber = request.PhoneNumber,
                DateOfBirth = request.DateOfBirth.ToDateTime(TimeOnly.MinValue),
                IsLinkedToAccount = true
            };
        }

        public static PatientProfile ToEntity(this CreatePatientProfileCommand request)
        {
            return new PatientProfile
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                PhoneNumber = request.PhoneNumber,
                DateOfBirth = request.DateOfBirth.ToDateTime(TimeOnly.MinValue),
                OwnerId = request.OwnerId,
                IsLinkedToAccount = true
            };
        }

        public static DoctorProfileDetailDto ToDetailDto(this DoctorProfile entity, string officeName)
        {
            return new DoctorProfileDetailDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                MiddleName = entity.MiddleName,
                Email = entity.Email,
                DateOfBirth = entity.DateOfBirth,
                Specialization = entity.Specialization,
                OfficeId = entity.OfficeId,
                OfficeName = officeName,
                CareerStartYear = entity.CareerStartYear,
                Status = entity.Status
            };
        }
    }
}
