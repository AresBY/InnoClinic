// Application/Common/Mappings/MappingProfile.cs
using AutoMapper;
using InnoClinic.Server.Application.DTOs;
using InnoClinic.Server.Domain.Entities;

namespace InnoClinic.Server.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Patient, PatientDto>();
    }
}
