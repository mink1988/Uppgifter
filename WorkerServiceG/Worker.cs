using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerServiceG
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
    
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            
            _logger.LogInformation("The service has been started");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            
            _logger.LogInformation("The service has been stopped");
            return base.StopAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Random r = new Random();
                int genRand = r.Next(-10, 30);

                if (genRand>20)
                {
                    _logger.LogInformation($"The limit has been reached! {genRand} degrees! It's nice weather outside!");
                }
               
                await Task.Delay(10 * 1000, stoppingToken);
            }
        }
    }
}
