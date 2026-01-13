using Microsoft.EntityFrameworkCore;
using PersonalJournalApp.Models;

namespace PersonalJournalApp.Data;

public class JournalDbContext : DbContext
{
    public DbSet<JournalEntry> JournalEntries { get; set; }
    public DbSet<Mood> Moods { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<JournalEntrySecondaryMood> EntrySecondaryMoods { get; set; }
    public DbSet<JournalEntryTag> EntryTags { get; set; }

    public JournalDbContext(DbContextOptions<JournalDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Composite Key for Secondary Moods
        modelBuilder.Entity<JournalEntrySecondaryMood>()
            .HasKey(sm => new { sm.JournalEntryId, sm.MoodId });

        // Composite Key for Tags
        modelBuilder.Entity<JournalEntryTag>()
            .HasKey(et => new { et.JournalEntryId, et.TagId });

        // Relationships
        modelBuilder.Entity<JournalEntry>()
            .HasOne(e => e.PrimaryMood)
            .WithMany()
            .HasForeignKey(e => e.PrimaryMoodId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed Moods
        modelBuilder.Entity<Mood>().HasData(Mood.GetPredefinedMoods().Select((m, i) => { m.Id = i + 1; return m; }));
        
        // Seed default tags
        modelBuilder.Entity<Tag>().HasData(Tag.GetPrebuiltTags().Select((t, i) => new Tag { Id = i + 1, Name = t }));
    }
}
