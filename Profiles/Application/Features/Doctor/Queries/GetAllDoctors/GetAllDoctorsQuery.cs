using InnoClinic.Offices.Application.DTOs;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.GetAllDoctors
{
    public class GetAllDoctorsQuery : IRequest<List<DoctorProfileDto>>
    {
    }
}
