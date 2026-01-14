using PersonalJournalApp.Services.Interfaces;

namespace PersonalJournalApp.Services;

public class ThemeService : IThemeService
{
    private const string ThemeKey = "is_dark_mode";
    public bool IsDarkMode { get; private set; }

    public event Action? OnChange;

    public Task InitializeAsync()
    {
        IsDarkMode = Preferences.Default.Get(ThemeKey, false);
        return Task.CompletedTask;
    }

    public void ToggleTheme()
    {
        IsDarkMode = !IsDarkMode;
        Preferences.Default.Set(ThemeKey, IsDarkMode);
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
