namespace PersonalJournalApp.Models;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public static List<string> GetPrebuiltTags() => new()
    {
        "Work", "Career", "Studies", "Family", "Friends", "Relationships", "Health", 
        "Fitness", "Personal Growth", "Self-care", "Hobbies", "Travel", "Nature", 
        "Finance", "Spirituality", "Birthday", "Holiday", "Vacation", "Celebration", 
        "Exercise", "Reading", "Writing", "Cooking", "Meditation", "Yoga", "Music", 
        "Shopping", "Parenting", "Projects", "Planning", "Reflection"
    };
}
