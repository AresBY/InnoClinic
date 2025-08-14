using InnoClinic.Offices.Domain.Enums;
using InnoClinic.Profiles.Application.DTOs;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.GetDoctorsForReceptionist
{
    public sealed class GetDoctorsForReceptionistQuery : IRequest<List<DoctorListItemDto>>
    {
        public string? FullName { get; set; }
        public DoctorSpecialization? Specialization { get; set; }
        public Guid? OfficeId { get; set; }
    }

}
