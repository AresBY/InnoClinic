using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Offices.Application.Features.Office.Commands;
using InnoClinic.Offices.Domain.Entities;
using MongoDB.Bson;
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
                PhotoUrl = office.PhotoUrl,
                City = office.City,
                Street = office.Street,
                HouseNumber = office.HouseNumber,
                OfficeNumber = office.OfficeNumber,
                Status = office.Status,
                RegistryPhoneNumber = office.RegistryPhoneNumber
            };
        }
        public static Office ToEntity(this CreateCommand command)
        {
            return new Office
            {
                Id = ObjectId.GenerateNewId().ToString(),
                PhotoUrl = command.PhotoUrl,
                City = command.City,
                Street = command.Street,
                HouseNumber = command.HouseNumber,
                OfficeNumber = command.OfficeNumber,
                RegistryPhoneNumber = command.RegistryPhoneNumber,
                Status = command.Status,
            };
        }

        public static Office ToEntity(this UpdateCommand command)
        {
            return new Office
            {
                Id = command.Id,
                PhotoUrl = command.PhotoUrl,
                City = command.City,
                Street = command.Street,
                HouseNumber = command.HouseNumber,
                OfficeNumber = command.OfficeNumber,
                RegistryPhoneNumber = command.RegistryPhoneNumber,
                Status = command.Status,
                CreatedAt = command.CreatedAt,
            };
        }
    }
}
