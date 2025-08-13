using InnoClinic.Profiles.Application.DTOs;
using InnoClinic.Profiles.Application.Interfaces;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Queries.GetOfficesForMapFromApi
{
    public sealed class GetOfficesForMapFromApiQueryHandler
        : IRequestHandler<GetOfficesForMapFromApiQuery, List<OfficeMapDto>>
    {
        private readonly IOfficeApiClient _officeApiClient;

        public GetOfficesForMapFromApiQueryHandler(IOfficeApiClient officeApiClient)
        {
            _officeApiClient = officeApiClient;
        }

        public async Task<List<OfficeMapDto>> Handle(GetOfficesForMapFromApiQuery request, CancellationToken cancellationToken)
        {
            return await _officeApiClient.GetOfficesForMapAsync(cancellationToken);
        }
    }
}
