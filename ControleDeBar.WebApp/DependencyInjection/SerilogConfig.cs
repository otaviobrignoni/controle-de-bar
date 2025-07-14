using Serilog;
using Serilog.Events;

namespace ControleDeBar.WebApp.DependencyInjection;

public static class SerilogConfig
{
    public static void AddSerilogConfig(this IServiceCollection services, ILoggingBuilder logging)
    {
        var caminhoAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        var caminhoLog = Path.Combine(caminhoAppData, "controle-de-bar", "log.log");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File(caminhoLog, LogEventLevel.Error)
            .CreateLogger();

        logging.ClearProviders();

        services.AddSerilog();
    }
}
