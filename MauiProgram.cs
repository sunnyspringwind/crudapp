using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using Microsoft.EntityFrameworkCore;
using PersonalJournalApp.Data;
using PersonalJournalApp.Services;
using PersonalJournalApp.Services.Interfaces;
using System.IO;

namespace PersonalJournalApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddMudServices();

        // Database Configuration
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "journal.db3");
        builder.Services.AddDbContext<JournalDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        // Services
        builder.Services.AddScoped<IJournalService, JournalService>();
        builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
        builder.Services.AddScoped<ISecurityService, SecurityService>();
        builder.Services.AddScoped<IExportService, ExportService>();
        builder.Services.AddScoped<IThemeService, ThemeService>();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
