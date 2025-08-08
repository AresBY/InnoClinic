using InnoClinic.Offices.Application.DTOs;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.GetDoctorsAll
{
    public class GetDoctorProfileAllQuery : IRequest<List<DoctorProfileDto>>
    {
    }
}
