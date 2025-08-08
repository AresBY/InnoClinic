using InnoClinic.Offices.Application.DTOs;
using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Application.Mappings;

using InnoClinicCommon.Exception;

using MediatR;

namespace InnoClinic.Profiles.Application.Features.Doctor.Commands.EditDoctorProfile
{
    public class EditDoctorOrReceptionistProfileByOwnCommandHandler : IRequestHandler<EditDoctorOrReceptionistProfileByOwnCommand, DoctorProfileDto>
    {
        private readonly IDoctorProfileRepository _repository;

        public EditDoctorOrReceptionistProfileByOwnCommandHandler(IDoctorProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<DoctorProfileDto> Handle(EditDoctorOrReceptionistProfileByOwnCommand request, CancellationToken cancellationToken)
        {
            var existingProfile = await _repository.GetDoctorProfileByUserIdAsync(request.Id, cancellationToken);
            if (existingProfile == null)
                throw new NotFoundException("Doctor profile not found");


            existingProfile.FirstName = request.FirstName;
            existingProfile.LastName = request.LastName;
            existingProfile.MiddleName = request.MiddleName;
            existingProfile.DateOfBirth = request.DateOfBirth;
            existingProfile.Specialization = request.Specialization;
            existingProfile.OfficeId = request.OfficeId;
            existingProfile.CareerStartYear = request.CareerStartYear;
            existingProfile.Status = request.Status;

            await _repository.UpdateAsync(existingProfile, cancellationToken);

            return existingProfile.ToDto();
        }
    }
}
