
using InvestCloud.MatrixCalculator.Application;
using InvestCloud.MatrixCalculator.Application.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

internal class Program
{
    static async Task Main(string[] args)
    {

        var builder = new ConfigurationBuilder();
        BuildConfiguration(builder);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Build())
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        var section = builder.Build().GetSection("Endpoints:Main");
        Log.Logger.Information("Application starting:");


        var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddSingleton<ILogger>(Log.Logger)
                        .AddTransient<IInvestCloudService, InvestCloudService>()
                        .AddTransient<IInvestCloudClient, InvestCloudClient>()
                        .AddHttpClient("InvestCloudClient", c => c.BaseAddress = new Uri(section.Value));

                })
                .UseSerilog()
                .Build();

        var svc = ActivatorUtilities.CreateInstance<InvestCloudService>(host.Services);
        await svc.Run(1000);


        static void BuildConfiguration(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }
    }
}