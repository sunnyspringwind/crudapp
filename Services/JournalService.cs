using Microsoft.EntityFrameworkCore;
using PersonalJournalApp.Data;
using PersonalJournalApp.Models;
using PersonalJournalApp.Services.Interfaces;

namespace PersonalJournalApp.Services;

public class JournalService : IJournalService
{
    private readonly JournalDbContext _context;
    private bool _isInitialized;

    public JournalService(JournalDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync()
    {
        if (_isInitialized) return;

        try
        {
            await _context.Database.EnsureCreatedAsync();
            _isInitialized = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database initialization failed: {ex.Message}");
        }
    }

    public async Task<JournalEntry?> GetEntryByDateAsync(DateTime date)
    {
        return await _context.JournalEntries
            .Include(e => e.PrimaryMood)
            .Include(e => e.Category)
            .Include(e => e.SecondaryMoods).ThenInclude(sm => sm.Mood)
            .Include(e => e.Tags).ThenInclude(et => et.Tag)
            .FirstOrDefaultAsync(e => e.Date.Date == date.Date);
    }

    public async Task<List<JournalEntry>> GetEntriesAsync(DateTime? startDate = null, DateTime? endDate = null, int? moodId = null, List<int>? tagIds = null, string? searchText = null)
    {
        var query = _context.JournalEntries
            .Include(e => e.PrimaryMood)
            .Include(e => e.Tags).ThenInclude(et => et.Tag)
            .AsQueryable();

        if (startDate.HasValue) query = query.Where(e => e.Date >= startDate.Value.Date);
        if (endDate.HasValue) query = query.Where(e => e.Date <= endDate.Value.Date);
        if (moodId.HasValue) query = query.Where(e => e.PrimaryMoodId == moodId.Value || e.SecondaryMoods.Any(sm => sm.MoodId == moodId.Value));

        if (tagIds != null && tagIds.Any())
        {
            query = query.Where(e => e.Tags.Any(et => tagIds.Contains(et.TagId)));
        }

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(e => e.Content.Contains(searchText));
        }

        return await query.OrderByDescending(e => e.Date).ToListAsync();
    }

    public async Task SaveEntryAsync(JournalEntry entry, List<int> secondaryMoodIds, List<int> tagIds)
    {
        var existingEntry = await _context.JournalEntries
            .Include(e => e.SecondaryMoods)
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(e => e.Id == entry.Id || e.Date.Date == entry.Date.Date);

        if (existingEntry != null)
        {
            existingEntry.Content = entry.Content;
            existingEntry.PrimaryMoodId = entry.PrimaryMoodId;
            existingEntry.CategoryId = entry.CategoryId;
            existingEntry.UpdatedAt = DateTime.Now;

            // Update secondary moods
            _context.EntrySecondaryMoods.RemoveRange(existingEntry.SecondaryMoods);
            foreach (var moodId in secondaryMoodIds.Take(2))
            {
                existingEntry.SecondaryMoods.Add(new JournalEntrySecondaryMood { MoodId = moodId });
            }

            // Update tags
            _context.EntryTags.RemoveRange(existingEntry.Tags);
            foreach (var tagId in tagIds)
            {
                existingEntry.Tags.Add(new JournalEntryTag { TagId = tagId });
            }

            _context.JournalEntries.Update(existingEntry);
        }
        else
        {
            entry.CreatedAt = DateTime.Now;
            entry.UpdatedAt = DateTime.Now;

            foreach (var moodId in secondaryMoodIds.Take(2))
            {
                entry.SecondaryMoods.Add(new JournalEntrySecondaryMood { MoodId = moodId });
            }

            foreach (var tagId in tagIds)
            {
                entry.Tags.Add(new JournalEntryTag { TagId = tagId });
            }

            _context.JournalEntries.Add(entry);
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteEntryAsync(int id)
    {
        var entry = await _context.JournalEntries.FindAsync(id);
        if (entry != null)
        {
            _context.JournalEntries.Remove(entry);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Mood>> GetAllMoodsAsync() => await _context.Moods.ToListAsync();
    public async Task<List<Tag>> GetAllTagsAsync() => await _context.Tags.ToListAsync();
    public async Task<List<Category>> GetAllCategoriesAsync() => await _context.Categories.ToListAsync();

    public async Task<Category> AddCategoryAsync(string name)
    {
        var category = new Category { Name = name };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }
}
