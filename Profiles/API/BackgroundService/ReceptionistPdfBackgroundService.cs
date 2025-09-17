using InnoClinic.Profiles.Application.DTOs;
using InnoClinic.Profiles.Application.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.API.BackgroundPdfService;
public class ReceptionistPdfBackgroundService : BackgroundService
{
    private readonly PdfGeneratorService _pdfGenerator;
    private readonly IServiceScopeFactory _scopeFactory;

    public ReceptionistPdfBackgroundService(
        PdfGeneratorService pdfGenerator,
        IServiceScopeFactory scopeFactory)
    {
        _pdfGenerator = pdfGenerator;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IReceptionistProfileRepository>();

                var receptionists = await repository.Query()
                    .Select(r => new ReceptionistProfileDto
                    {
                        Id = r.Id,
                        FirstName = r.FirstName,
                        MiddleName = r.MiddleName,
                        LastName = r.LastName
                    })
                    .ToListAsync(stoppingToken);

                _pdfGenerator.GenerateReceptionistsPdf(receptionists);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка генерации PDF: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
        }
    }
}
