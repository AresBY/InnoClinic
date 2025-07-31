using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoClinic.Offices.Application.DTOs
{
    public record OfficeDto
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Address { get; init; }
        public bool Status { get; init; }
        public string RegistryPhoneNumber { get; init; }
    }
}
