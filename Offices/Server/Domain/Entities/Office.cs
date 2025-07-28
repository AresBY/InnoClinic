using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InnoClinic.Offices.Domain.Entities
{
    public class Office
    {
        public string Id { get; set; } = default!;
        public string? PhotoUrl { get; set; }
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string HouseNumber { get; set; } = default!;
        public string? OfficeNumber { get; set; }
        public string RegistryPhoneNumber { get; set; } = default!;
        public bool Status { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
