using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Events;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using DAL.EF;
namespace wg_forge_backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            //.WriteTo.File(new RenderedCompactJsonFormatter(), "/log.ndjson")
            .WriteTo.File(new RenderedCompactJsonFormatter(), 
              path: Path.Combine(Environment.CurrentDirectory, @"logs", @"skilliam.ndjson"),
              rollingInterval: RollingInterval.Day,
              rollOnFileSizeLimit: true,
              fileSizeLimitBytes: 123456)
            .CreateLogger();
            try
            {
                var host = CreateHostBuilder(args).Build();
                //Microsoft suggests doing so  https://docs.microsoft.com/ru-ru/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli
                //var scope = host.Services.CreateScope();                
                //var db = scope.ServiceProvider.GetRequiredService<CatContext>();
                //db.Database.Migrate();
                //scope.Dispose();
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()/*.UseKestrel(i => i.Limits.MaxConcurrentConnections = 600)*/;

                });
    }
}
