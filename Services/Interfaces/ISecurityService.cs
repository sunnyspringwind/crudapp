namespace PersonalJournalApp.Services.Interfaces;

public interface ISecurityService
{
    Task<bool> IsPinSetAsync();
    Task<bool> VerifyPinAsync(string pin);
    Task SetPinAsync(string pin);
    Task ClearPinAsync();
}
