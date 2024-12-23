using Serilog;
using DemoCRUD;
public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build())
            .CreateLogger();
        var builder = CreateBuilder(args).Build();
        builder.Run();
    }
    public static IHostBuilder CreateBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureWebHostDefaults(webBuilders => { webBuilders.UseStartup<Startup>(); });
}