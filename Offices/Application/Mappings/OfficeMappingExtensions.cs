using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Offices.Application.Features.Office.Commands.CreateOffice;
using InnoClinic.Offices.Application.Features.Office.Commands.UpdateOffice;
using InnoClinic.Offices.Domain.Entities;

using MongoDB.Bson;

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
        public static Office ToEntity(this CreateOfficeCommand command)
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

        public static Office ToEntity(this UpdateOfficeCommand command)
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

        public static OfficeMapDto ToMapDto(this Office office)
        {
            return new OfficeMapDto
            {
                Id = office.Id,
                Name = $"{office.City}, {office.Street} {office.HouseNumber}" + (string.IsNullOrEmpty(office.OfficeNumber) ? "" : $", {office.OfficeNumber}"),
                Address = $"{office.City}, {office.Street} {office.HouseNumber}" + (string.IsNullOrEmpty(office.OfficeNumber) ? "" : $", {office.OfficeNumber}"),
                PhotoUrl = office.PhotoUrl,
                Latitude = office.Latitude,
                Longitude = office.Longitude
            };
        }
    }
}
