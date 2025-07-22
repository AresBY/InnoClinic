using InnoClinic.Server.Application.DTOs;
using InnoClinic.Server.Domain.Entities;

namespace InnoClinic.Server.Application.Extensions
{
    public static class PatientMappingExtensions
    {
        public static PatientDto ToDto(this Patient patient)
        {
            return new PatientDto
            {
                Id = patient.Id,
                Email = patient.Email,
                IsEmailConfirmed = patient.IsEmailConfirmed
            };
        }
    }
}
