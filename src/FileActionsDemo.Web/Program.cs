using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace FileActionsDemo.Web;

//https://community.abp.io/posts/file-uploaddownload-with-blob-storage-system-in-asp.net-core-abp-framework-d01cbe12
//docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=rm" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
//https://www.talkingdotnet.com/how-to-increase-file-upload-size-asp-net-core/#:~:text=ASP.NET%20Core%202.0%20enforces,increase%20the%20default%20allowed%20limit.

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Async(c => c.File("Logs/logs.txt"))
#if DEBUG
            .WriteTo.Async(c => c.Console())
#endif
            .CreateLogger();

        try
        {
            Log.Information("Starting web host.");
            var builder = WebApplication.CreateBuilder(args);
            //builder.WebHost.ConfigureKestrel(serverOptions =>
            //{
            //    //serverOptions.Limits.MaxRequestBodySize = 60000000;
            //    serverOptions.Limits.MaxRequestBodySize = 2000;
            //});
            //builder.Services.Configure<IISServerOptions>(options =>
            //{
            //    options.MaxRequestBodySize = 2000; // or your desired value
            //});

            builder.Host
                .AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseSerilog();
            
            await builder.AddApplicationAsync<FileActionsDemoWebModule>();
            
            var app = builder.Build();
            
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
