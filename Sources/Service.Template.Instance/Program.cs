using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Service.Template.Instance
{
    /// <summary>
    /// Точка входа в приложение.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Точка входа в приложение.
        /// </summary>
        /// <param name="args">Аргументы запуска приложения.</param>
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(@"logs\\startup-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Log.Information("Начинается запуск приложения.");
                if (args.Contains("--service") || args.Contains("-s"))
                {
                    Log.Information("Найдены флаги '--service' или '-s'. Приложение будет запущено как windows-служба.");
                    string contentRootPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName);

                    Log.Information("Начинается построение хоста приложения.");

                    IHost host = Host.CreateDefaultBuilder(args)
                        .UseSerilog()
                        .ConfigureWebHostDefaults(webBuilder =>
                        {
                            webBuilder.UseStartup<Startup>();
                        })
                        .UseContentRoot(contentRootPath)
                        .Build();

                    Log.Information("Построение хоста приложения успешно завершено.");

                    host.Run();
                }
                else
                {
                    Log.Information("Приложение будет запущено как консольное приложение. Для того, что бы запустить приложение как windows-службу, передайте флаги '--service' или '-s'.");
                    string contentRootPath = Directory.GetCurrentDirectory();

                    Log.Information("Начинается построение хоста приложения.");

                    IHost host = Host.CreateDefaultBuilder(args)
                        .UseSerilog()
                        .ConfigureWebHostDefaults(webBuilder =>
                        {
                            webBuilder.UseStartup<Startup>();
                        })
                        .UseContentRoot(contentRootPath)
                        .Build();

                    Log.Information("Построение хоста приложения успешно завершено.");

                    host.Run();
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Приложение неожиданно завершено.");
            }
            finally
            {
                Log.Information("Приложение завершается.");
                Log.CloseAndFlush();
            }
        }
    }
}