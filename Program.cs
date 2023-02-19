using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MovieAPi.Infrastructures.Persistence.Services;
using MovieAPi.Infrastructures.QueueService;
using MovieAPi.Infrastructures.Worker;
using MovieAPi.Interfaces.Persistence.QueueService;

namespace MovieAPi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Run();
        }

        public static IHost CreateHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<GetPopularMovieService>();
                    services.AddHostedService<QueuedHostedService>();
                    services.AddSingleton<IBackgroundTaskQueue>(_ =>
                    {
                        if (!int.TryParse(hostContext.Configuration["BackgroundTaskQueue:MaxQueueLength"],
                                out var maxQueueLength))
                        {
                            maxQueueLength = 100;
                        }
                        return new DefaultBackgroundTaskQueue(maxQueueLength);
                    });
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .Build();
            
            var monitorLoopService = host.Services.GetRequiredService<GetPopularMovieService>();
            monitorLoopService.StartMonitorLoop();

            return host;
        }
    }
}