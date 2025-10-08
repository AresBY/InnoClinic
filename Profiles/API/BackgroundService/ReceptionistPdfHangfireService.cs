using InnoClinic.Profiles.Application.DTOs;
using InnoClinic.Profiles.Application.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.API.BackgroundPdfService
{
    public class ReceptionistPdfHangfireService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly PdfGeneratorService _pdfGenerator;

        public ReceptionistPdfHangfireService(IServiceScopeFactory scopeFactory, PdfGeneratorService pdfGenerator)
        {
            _scopeFactory = scopeFactory;
            _pdfGenerator = pdfGenerator;
        }

        public async Task GeneratePdfForAllReceptionists()
        {
            using var scope = _scopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IReceptionistProfileRepository>();

            var receptionists = await repository.Query()
                .Select(r => new ReceptionistProfileDto
                {
                    Id = r.Id,
                    FirstName = r.FirstName,
                    MiddleName = r.MiddleName,
                    LastName = r.LastName,
                })
                .ToListAsync();

            _pdfGenerator.GenerateReceptionistsPdf(receptionists);

            //repeat
            Hangfire.BackgroundJob.Schedule<ReceptionistPdfHangfireService>(
                x => x.GeneratePdfForAllReceptionists(),
                TimeSpan.FromSeconds(10)
            );
        }
    }
}
