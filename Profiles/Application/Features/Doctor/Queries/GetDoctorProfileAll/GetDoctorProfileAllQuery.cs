using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Offices.Domain.Enums;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.GetDoctorsAll
{
    public sealed class GetDoctorProfileAllQuery : IRequest<List<DoctorProfileDto>>
    {
        public DoctorSpecialization? Specialization { get; }

        public GetDoctorProfileAllQuery(DoctorSpecialization? specialization = null)
        {
            Specialization = specialization;
        }
    }
}
