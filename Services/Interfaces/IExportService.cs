using PersonalJournalApp.Models;

namespace PersonalJournalApp.Services.Interfaces;

public interface IExportService
{
    Task<byte[]> ExportToPdfAsync(List<JournalEntry> entries);
}
