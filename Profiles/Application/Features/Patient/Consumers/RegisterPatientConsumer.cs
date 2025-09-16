using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Profiles.Domain.Entities;

using MassTransit;

using static InnoClinicCommon.DTOs.RegisterPatientEntities;

namespace InnoClinic.Profiles.Application.Features.Patient.Consumers
{
    public class RegisterPatientConsumer : IConsumer<RegisterPatient>
    {
        private readonly IPatientProfileRepository _patientRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public RegisterPatientConsumer(
            IPatientProfileRepository patientRepository,
            IPublishEndpoint publishEndpoint)
        {
            _patientRepository = patientRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<RegisterPatient> context)
        {
            try
            {
                var patient = new PatientProfile
                {
                    Id = Guid.NewGuid(),
                    FirstName = "FirstName",
                    LastName = "Lastname",
                    PhoneNumber = "+1234567890"
                };

                await _patientRepository.AddAsync(patient, context.CancellationToken);

                await _publishEndpoint.Publish<ProfileCreated>(new
                {
                    CorrelationId = context.Message.CorrelationId,
                    ProfileId = patient.Id
                }, context.CancellationToken);
            }
            catch (Exception ex)
            {
                await _publishEndpoint.Publish<ProfileCreationFailed>(new
                {
                    CorrelationId = context.Message.CorrelationId,
                    Reason = ex.Message
                }, context.CancellationToken);
            }
        }
    }

}
