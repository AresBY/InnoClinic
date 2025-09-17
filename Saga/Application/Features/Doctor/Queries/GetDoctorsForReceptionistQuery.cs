using InnoClinic.Saga.Contract;

using MediatR;

namespace InnoClinic.Saga.Application.Features.Doctor.Queries
{
    public class GetDoctorsForReceptionistQuery : IRequest<List<DoctorDto>>
    {
        public string? FullName { get; init; }
        public Guid? OfficeId { get; init; }
        public int? SpecializationId { get; init; }
    }
}
