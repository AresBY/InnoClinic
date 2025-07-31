using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Offices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoClinic.Offices.Application.Mappings
{
    public static class OfficeMappingExtensions
    {
        public static OfficeDto ToDto(this Office office)
        {
            return new OfficeDto
            {
                Id = office.Id,
                Status = office.Status,
                RegistryPhoneNumber = office.RegistryPhoneNumber
            };
        }
    }
}
