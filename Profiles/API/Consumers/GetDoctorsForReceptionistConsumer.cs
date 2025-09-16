using InnoClinic.Profiles.Application.Interfaces.Repositories;
using InnoClinic.Saga.Contract;

using MassTransit;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.API.Consumers
{
    public class GetDoctorsForReceptionistConsumer : IConsumer<GetDoctorsForReceptionistRequest>
    {
        private readonly IDoctorProfileRepository _doctorRepository;

        public GetDoctorsForReceptionistConsumer(IDoctorProfileRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task Consume(ConsumeContext<GetDoctorsForReceptionistRequest> context)
        {
            var request = context.Message;
            var query = _doctorRepository.Query();

            if (!string.IsNullOrWhiteSpace(request.FullName))
            {
                var fullNameLower = request.FullName.Trim().ToLower();
                query = query.Where(d =>
                    ((d.FirstName ?? "") + " " + (d.LastName ?? "") + " " + (d.MiddleName ?? ""))
                        .ToLower()
                        .Contains(fullNameLower)
                );
            }

            if (request.Specialization.HasValue)
            {
                query = query.Where(d => (int)d.Specialization == request.Specialization.Value);
            }

            if (request.OfficeId.HasValue)
            {
                query = query.Where(d => d.OfficeId == request.OfficeId.Value);
            }

            var doctors = await query.ToListAsync(context.CancellationToken);

            var response = new GetDoctorsForReceptionistResponse(
            doctors.Select(d => new DoctorDto
            {
                Id = d.Id,
                FullName = $"{d.FirstName} {d.LastName} {d.MiddleName}".Trim(),
                Specialization = ((int)d.Specialization).ToString(),
                Status = ((int)d.Status).ToString(),
                DateOfBirth = d.DateOfBirth.DateTime,
                OfficeId = d.OfficeId,
                OfficeAddress = string.Empty
            }).ToList()
        );

            await context.RespondAsync(response);

            Console.WriteLine($"[Consumer] RespondAsync called for request {request.FullName}");
        }
    }
}
