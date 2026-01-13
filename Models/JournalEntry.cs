using System.ComponentModel.DataAnnotations;

namespace PersonalJournalApp.Models;

public class JournalEntry
{
    public int Id { get; set; }

    [Required]
    public DateTime Date { get; set; } = DateTime.Today;

    [Required]
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public int PrimaryMoodId { get; set; }
    public virtual Mood PrimaryMood { get; set; } = null!;

    public int? CategoryId { get; set; }
    public virtual Category? Category { get; set; }

    public virtual ICollection<JournalEntrySecondaryMood> SecondaryMoods { get; set; } = new List<JournalEntrySecondaryMood>();
    public virtual ICollection<JournalEntryTag> Tags { get; set; } = new List<JournalEntryTag>();

    public int WordCount => string.IsNullOrWhiteSpace(Content) 
        ? 0 
        : Content.Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
}

public class JournalEntrySecondaryMood
{
    public int JournalEntryId { get; set; }
    public int MoodId { get; set; }
    public virtual JournalEntry JournalEntry { get; set; } = null!;
    public virtual Mood Mood { get; set; } = null!;
}

public class JournalEntryTag
{
    public int JournalEntryId { get; set; }
    public int TagId { get; set; }
    public virtual JournalEntry JournalEntry { get; set; } = null!;
    public virtual Tag Tag { get; set; } = null!;
}
