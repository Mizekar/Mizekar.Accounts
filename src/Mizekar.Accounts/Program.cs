using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Mizekar.Accounts
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            var configuration = host.Services.GetRequiredService<IConfiguration>();
            //if (seed)
            //{
            //  var connectionString = configuration["db_connection"];
            //  SeedData.EnsureSeedData(connectionString);
            //  return;
            //}

            var elasticUri = configuration["ElasticConfigurationUri"];
            Log.Logger = new LoggerConfiguration()
                //.ReadFrom.Configuration(configuration) //Install-Package Serilog.Settings.Configuration
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithMachineName()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
                {
                    AutoRegisterTemplate = true,
                    CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true)
                    //FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                    //EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                    //                   EmitEventFailureHandling.WriteToFailureSink |
                    //                   EmitEventFailureHandling.RaiseCallback,
                    //FailureSink = new FileSink("./failures.txt", new JsonFormatter(), null)
                })
                .CreateLogger();

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                })
                .UseSerilog();
    }
}
