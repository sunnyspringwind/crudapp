using PersonalJournalApp.Services.Interfaces;
using BCrypt.Net;

namespace PersonalJournalApp.Services;

public class SecurityService : ISecurityService
{
    private const string PinKey = "user_pin_hash";

    public Task<bool> IsPinSetAsync()
    {
        return Task.FromResult(Preferences.Default.ContainsKey(PinKey));
    }

    public Task<bool> VerifyPinAsync(string pin)
    {
        var hash = Preferences.Default.Get(PinKey, string.Empty);
        if (string.IsNullOrEmpty(hash)) return Task.FromResult(true);

        try
        {
            return Task.FromResult(BCrypt.Net.BCrypt.Verify(pin, hash));
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public Task SetPinAsync(string pin)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(pin);
        Preferences.Default.Set(PinKey, hash);
        return Task.CompletedTask;
    }

    public Task ClearPinAsync()
    {
        Preferences.Default.Remove(PinKey);
        return Task.CompletedTask;
    }
}
