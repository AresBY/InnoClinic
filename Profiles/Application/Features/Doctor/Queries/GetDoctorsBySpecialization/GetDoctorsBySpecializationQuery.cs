using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Offices.Domain.Enums;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.GetDoctorsBySpecialization
{
    public sealed class GetDoctorsBySpecializationQuery : IRequest<List<DoctorProfileDto>>
    {
        public DoctorSpecialization? Specialization { get; }

        public GetDoctorsBySpecializationQuery(DoctorSpecialization? specialization)
        {
            Specialization = specialization;
        }
    }
}