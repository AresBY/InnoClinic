using InnoClinic.Profiles.Application.DTOs;
using InnoClinic.Profiles.Application.Interfaces.Repositories;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Application.Features.Receptionist.Queries.GetAllReceptionists
{
    public sealed class GetAllReceptionistsQueryHandler : IRequestHandler<GetAllReceptionistsQuery, List<ReceptionistProfileDto>>
    {
        private readonly IReceptionistProfileRepository _repository;

        public GetAllReceptionistsQueryHandler(IReceptionistProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ReceptionistProfileDto>> Handle(GetAllReceptionistsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.Query()
                .Select(r => new ReceptionistProfileDto
                {
                    Id = r.Id,
                    FirstName = r.FirstName,
                    LastName = r.LastName,
                    MiddleName = r.MiddleName
                })
                .ToListAsync(cancellationToken);
        }
    }
}
