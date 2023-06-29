using System;
using AirlineSendAgent.App;
using AirlineSendAgent.Client;
using AirlineSendAgent.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AirlineSendAgent
{
    class Program
    {
        public Program(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        static void Main(string[] args)
        {

            var host = Host.CreateDefaultBuilder()
                    .ConfigureServices((context, services) => {
                        services.AddSingleton<IAppHost, AppHost>();
                        services.AddSingleton<IWebhookClient, WebhookClient>();
                        services.AddDbContext<SendAgentDBContext>(opt => opt.UseSqlite(
                            context.Configuration.GetConnectionString("Default")));
                        services.AddHttpClient();
                    }).Build();

            host.Services.GetService<IAppHost>().Run();
        }
    }
}
