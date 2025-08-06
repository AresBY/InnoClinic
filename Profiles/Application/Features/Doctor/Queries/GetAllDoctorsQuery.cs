using InnoClinic.Offices.Application.DTOs;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries
{
    public class GetAllDoctorsQuery : IRequest<List<DoctorProfileDto>>
    {
    }
}
