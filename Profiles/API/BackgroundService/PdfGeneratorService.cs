using InnoClinic.Profiles.Application.DTOs;

using QuestPDF.Fluent;
using QuestPDF.Infrastructure;


namespace InnoClinic.Profiles.API.BackgroundPdfService;

public class PdfGeneratorService
{
    public void GenerateReceptionistsPdf(List<ReceptionistProfileDto> receptionists)
    {
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        var filePath = Path.Combine(desktopPath, "Receptionists.pdf");

        if (File.Exists(filePath))
            File.Delete(filePath);

        QuestPDF.Settings.License = LicenseType.Community;

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(QuestPDF.Helpers.PageSizes.A4);
                page.Margin(20);

                // Header
                page.Header().Column(column =>
                {
                    column.Item().Text("Receptionists Report")
                        .FontFamily("Helvetica")
                        .Bold()
                        .FontSize(20);

                    column.Item().Text($"Обновлено: {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
                        .FontFamily("Helvetica")
                        .FontSize(12);
                });

                // Content
                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(); // ID
                        columns.RelativeColumn(); // Full Name
                        columns.RelativeColumn(); // Office Address
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("ID").FontFamily("Helvetica").Bold();
                        header.Cell().Text("Full Name").FontFamily("Helvetica").Bold();
                        header.Cell().Text("Office Address").FontFamily("Helvetica").Bold();
                    });

                    foreach (var r in receptionists)
                    {
                        var fullName = $"{r.FirstName ?? "-"} {r.MiddleName ?? ""} {r.LastName ?? "-"}".Trim();
                        table.Cell().Text(r.Id.ToString()).FontFamily("Helvetica");
                        table.Cell().Text(fullName).FontFamily("Helvetica");
                        table.Cell().Text(r.OfficeAddress ?? "-").FontFamily("Helvetica");
                    }
                });
            });
        })
        .GeneratePdf(filePath);

        Console.WriteLine($"PDF обновлён: {filePath} ({DateTime.Now:yyyy-MM-dd HH:mm:ss})");
    }
}
