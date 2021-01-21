using System;
using System.IO;
using System.Threading.Tasks;
using KeePass;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DIBasic.ConsoleTemplate.NET5
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            // Default logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .CreateLogger();

            // Dependency injection configuration
            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(BuildConfig)
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddTransient<MainApp>()
                        .SetupKeePassServices(context.Configuration)
                        .AddOptions();
                })
                .UseSerilog()
                .Build();

            // Actual main application start
            try
            {
                var mainAppInstance = ActivatorUtilities.CreateInstance<MainApp>(host.Services);
                await mainAppInstance.Run(args);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application crashed! Logging unhandled exceptions now.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// All optional or required json files can be initialized here, so they will be accessible from <see cref="IConfiguration"/>
        /// </summary>
        /// <param name="builder"></param>
        private static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables()
                .AddUserSecrets(typeof(Program).Assembly, optional: true);
        }
    }
}