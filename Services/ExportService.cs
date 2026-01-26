using PersonalJournalApp.Models;
using PersonalJournalApp.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

namespace PersonalJournalApp.Services;

public class ExportService : IExportService
{
    public ExportService()
    {
        // QuestPDF requires license configuration
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public async Task<byte[]> ExportToPdfAsync(List<JournalEntry> entries)
    {
        return await Task.Run(() =>
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(QuestPDF.Helpers.Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Text("My Journal").FontSize(20).Bold().FontColor(QuestPDF.Helpers.Colors.Blue.Medium);

                    page.Content().Column(column =>
                    {
                        column.Spacing(10);

                        foreach (var entry in entries.OrderBy(e => e.Date))
                        {
                            column.Item().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2).PaddingVertical(5).Column(entryCol =>
                            {
                                entryCol.Item().Row(row =>
                                {
                                    row.RelativeItem().Text(entry.Date.ToString("MMMM dd, yyyy")).Bold();
                                    row.AutoItem().Text(entry.PrimaryMood?.Emoji ?? "");
                                });

                                entryCol.Item().Text(entry.Content);
                                
                                if (entry.Tags.Any())
                                {
                                    var tags = string.Join(", ", entry.Tags.Select(t => t.Tag.Name));
                                    entryCol.Item().Text($"Tags: {tags}").FontSize(10).Italic();
                                }
                            });
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
                });
            }).GeneratePdf();
        });
    }
}
