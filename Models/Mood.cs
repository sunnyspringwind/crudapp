namespace PersonalJournalApp.Models;

public enum MoodCategory
{
    Positive,
    Neutral,
    Negative
}

public class Mood
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public MoodCategory Category { get; set; }
    public string Emoji { get; set; } = string.Empty;

    public static List<Mood> GetPredefinedMoods() => new()
    {
        new Mood { Name = "Happy", Category = MoodCategory.Positive, Emoji = "😊" },
        new Mood { Name = "Excited", Category = MoodCategory.Positive, Emoji = "🤩" },
        new Mood { Name = "Relaxed", Category = MoodCategory.Positive, Emoji = "😌" },
        new Mood { Name = "Grateful", Category = MoodCategory.Positive, Emoji = "🙏" },
        new Mood { Name = "Confident", Category = MoodCategory.Positive, Emoji = "😎" },
        new Mood { Name = "Calm", Category = MoodCategory.Neutral, Emoji = "😐" },
        new Mood { Name = "Thoughtful", Category = MoodCategory.Neutral, Emoji = "🤔" },
        new Mood { Name = "Curious", Category = MoodCategory.Neutral, Emoji = "🧐" },
        new Mood { Name = "Nostalgic", Category = MoodCategory.Neutral, Emoji = "⏳" },
        new Mood { Name = "Bored", Category = MoodCategory.Neutral, Emoji = "😑" },
        new Mood { Name = "Sad", Category = MoodCategory.Negative, Emoji = "😔" },
        new Mood { Name = "Angry", Category = MoodCategory.Negative, Emoji = "😠" },
        new Mood { Name = "Stressed", Category = MoodCategory.Negative, Emoji = "😫" },
        new Mood { Name = "Lonely", Category = MoodCategory.Negative, Emoji = "😶" },
        new Mood { Name = "Anxious", Category = MoodCategory.Negative, Emoji = "😰" }
    };
}
