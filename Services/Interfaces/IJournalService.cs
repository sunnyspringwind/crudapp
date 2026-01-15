using PersonalJournalApp.Models;

namespace PersonalJournalApp.Services.Interfaces;

public interface IJournalService
{
    Task<JournalEntry?> GetEntryByDateAsync(DateTime date);
    Task<List<JournalEntry>> GetEntriesAsync(DateTime? startDate = null, DateTime? endDate = null, int? moodId = null, List<int>? tagIds = null, string? searchText = null);
    Task SaveEntryAsync(JournalEntry entry, List<int> secondaryMoodIds, List<int> tagIds);
    Task DeleteEntryAsync(int id);
    Task<List<Mood>> GetAllMoodsAsync();
    Task<List<Tag>> GetAllTagsAsync();
    Task<List<Category>> GetAllCategoriesAsync();
    Task InitializeAsync();
    Task<Category> AddCategoryAsync(string name);
}
