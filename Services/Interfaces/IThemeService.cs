namespace PersonalJournalApp.Services.Interfaces;

public interface IThemeService
{
    bool IsDarkMode { get; }
    event Action OnChange;
    void ToggleTheme();
    Task InitializeAsync();
}
