

namespace InnoClinic.Saga.Contract
{
    public record DoctorDto(
       Guid Id,
       string FullName,
       int Specialization,
       int Status,
       DateTime DateOfBirth,
       Guid OfficeId,
       string OfficeAddress
   );

    public record GetDoctorsForReceptionistRequest(
        string? FullName,
        Guid? OfficeId,
        int? Specialization
    );

    public record GetDoctorsForReceptionistResponse(
        List<DoctorDto> Doctors
    );
}
