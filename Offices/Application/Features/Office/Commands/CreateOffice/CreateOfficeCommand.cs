using MediatR;

namespace InnoClinic.Offices.Application.Features.Office.Commands.CreateOffice
{
    public sealed class CreateOfficeCommand : IRequest<string>
    {
        public string? PhotoUrl { get; set; }
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string HouseNumber { get; set; } = default!;
        public string? OfficeNumber { get; set; }
        public string RegistryPhoneNumber { get; set; } = default!;
        public bool Status { get; set; } = true;
    }
}
