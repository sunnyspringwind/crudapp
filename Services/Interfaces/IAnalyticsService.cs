using PersonalJournalApp.Models;

namespace PersonalJournalApp.Services.Interfaces;

public interface IAnalyticsService
{
    Task<Dictionary<MoodCategory, double>> GetMoodDistributionAsync(DateTime? start = null, DateTime? end = null);
    Task<Mood?> GetMostFrequentMoodAsync(DateTime? start = null, DateTime? end = null);
    Task<int> GetCurrentStreakAsync();
    Task<int> GetLongestStreakAsync();
    Task<List<DateTime>> GetMissedDaysAsync(DateTime start, DateTime end);
    Task<Dictionary<string, int>> GetMostUsedTagsAsync(int count = 10);
    Task<List<(DateTime Date, int WordCount)>> GetWordCountTrendsAsync(DateTime start, DateTime end);
}
