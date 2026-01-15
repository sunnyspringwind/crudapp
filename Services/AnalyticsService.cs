using Microsoft.EntityFrameworkCore;
using PersonalJournalApp.Data;
using PersonalJournalApp.Models;
using PersonalJournalApp.Services.Interfaces;

namespace PersonalJournalApp.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly JournalDbContext _context;

    public AnalyticsService(JournalDbContext context)
    {
        _context = context;
    }

    public async Task<Dictionary<MoodCategory, double>> GetMoodDistributionAsync(DateTime? start = null, DateTime? end = null)
    {
        var entries = await GetFilteredEntries(start, end);
        if (!entries.Any()) return new();

        var total = entries.Count;
        return entries
            .GroupBy(e => e.PrimaryMood.Category)
            .ToDictionary(g => g.Key, g => (double)g.Count() / total * 100);
    }

    public async Task<Mood?> GetMostFrequentMoodAsync(DateTime? start = null, DateTime? end = null)
    {
        var entries = await GetFilteredEntries(start, end);
        return entries
            .GroupBy(e => e.PrimaryMood)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefault();
    }

    public async Task<int> GetCurrentStreakAsync()
    {
        var dates = await _context.JournalEntries
            .OrderByDescending(e => e.Date)
            .Select(e => e.Date.Date)
            .ToListAsync();

        if (!dates.Any()) return 0;

        int streak = 0;
        var current = DateTime.Today;

        if (dates.First() < current.AddDays(-1)) return 0;

        foreach (var date in dates)
        {
            if (date == current || date == current.AddDays(-1))
            {
                if (date == current) streak++;
                else if (streak > 0 || date == current.AddDays(-1)) { streak++; current = date; }
            }
            else if (date == current.AddDays(-1))
            {
                streak++;
                current = date;
            }
            else break;
        }

        // Simpler streak logic
        streak = 0;
        var checkDate = dates.Contains(DateTime.Today) ? DateTime.Today : DateTime.Today.AddDays(-1);
        if (!dates.Contains(checkDate)) return 0;

        while (dates.Contains(checkDate))
        {
            streak++;
            checkDate = checkDate.AddDays(-1);
        }

        return streak;
    }

    public async Task<int> GetLongestStreakAsync()
    {
        var dates = await _context.JournalEntries
            .OrderByDescending(e => e.Date)
            .Select(e => e.Date.Date)
            .Distinct()
            .ToListAsync();

        if (!dates.Any()) return 0;

        int longest = 0;
        int current = 0;
        DateTime? lastDate = null;

        foreach (var date in dates.OrderBy(d => d))
        {
            if (lastDate == null || date == lastDate.Value.AddDays(1))
            {
                current++;
            }
            else
            {
                longest = Math.Max(longest, current);
                current = 1;
            }
            lastDate = date;
        }

        return Math.Max(longest, current);
    }

    public async Task<List<DateTime>> GetMissedDaysAsync(DateTime start, DateTime end)
    {
        var entries = await _context.JournalEntries
            .Where(e => e.Date >= start.Date && e.Date <= end.Date)
            .Select(e => e.Date.Date)
            .ToListAsync();

        var missed = new List<DateTime>();
        for (var d = start.Date; d <= end.Date; d = d.AddDays(1))
        {
            if (!entries.Contains(d)) missed.Add(d);
        }
        return missed;
    }

    public async Task<Dictionary<string, int>> GetMostUsedTagsAsync(int count = 10)
    {
        return await _context.EntryTags
            .Include(et => et.Tag)
            .GroupBy(et => et.Tag.Name)
            .OrderByDescending(g => g.Count())
            .Take(count)
            .ToDictionaryAsync(g => g.Key, g => g.Count());
    }

    public async Task<List<(DateTime Date, int WordCount)>> GetWordCountTrendsAsync(DateTime start, DateTime end)
    {
        var entries = await _context.JournalEntries
            .Where(e => e.Date >= start.Date && e.Date <= end.Date)
            .OrderBy(e => e.Date)
            .ToListAsync();

        return entries.Select(e => (e.Date, e.WordCount)).ToList();
    }

    private async Task<List<JournalEntry>> GetFilteredEntries(DateTime? start, DateTime? end)
    {
        var query = _context.JournalEntries.Include(e => e.PrimaryMood).AsQueryable();
        if (start.HasValue) query = query.Where(e => e.Date >= start.Value.Date);
        if (end.HasValue) query = query.Where(e => e.Date <= end.Value.Date);
        return await query.ToListAsync();
    }
}
