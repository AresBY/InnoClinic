using InnoClinic.Profiles.Application.DTOs;
using InnoClinic.Profiles.Application.Interfaces.Repositories;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Application.Features.Patient.Queries.SearchPatients
{
    public sealed class SearchPatientsQueryHandler : IRequestHandler<SearchPatientsQuery, List<PatientListItemDto>>
    {
        private readonly IPatientProfileRepository _patientRepository;

        public SearchPatientsQueryHandler(IPatientProfileRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<List<PatientListItemDto>> Handle(SearchPatientsQuery request, CancellationToken cancellationToken)
        {
            var query = _patientRepository.Query();

            if (!string.IsNullOrWhiteSpace(request.FullName))
            {
                var searchLower = request.FullName.Trim().ToLower();
                query = query.Where(p =>
                    ((p.FirstName ?? "").ToLower() + " " + (p.LastName ?? "").ToLower() + " " + (p.MiddleName ?? "").ToLower())
                    .Contains(searchLower)
                );
            }

            var patients = await query.ToListAsync(cancellationToken);

            return patients.Select(p => new PatientListItemDto
            {
                Id = p.Id,
                FullName = $"{p.FirstName} {p.LastName} {p.MiddleName}".Trim(),
                DateOfBirth = p.DateOfBirth,
                PhoneNumber = p.PhoneNumber
            }).ToList();
        }
    }
}
