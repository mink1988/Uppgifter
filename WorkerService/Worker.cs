using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WorkerServiceVG
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private string _url = "http://api.openweathermap.org/data/2.5/weather?q=orebro&units=metric&appid=a627a0c523b7b9134ce9b828fcd748f3";
        private HttpClient _client;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override  Task StartAsync(CancellationToken cancellationToken)
        {
            _client = new HttpClient();
            _logger.LogInformation("The service has been started");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            
            _logger.LogInformation("The service has been stopped");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var response = await _client.GetAsync(_url);
                    var json = await response.Content.ReadAsStringAsync();
                    var forecast = JsonConvert.DeserializeObject<Forecast>(json);

                    if (forecast.main.temp>10)
                    {
                        _logger.LogInformation($"The limit has been reached! {forecast.main.temp} degrees!");
                    }
                      
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                }
                await Task.Delay(60 * 1000, cancellationToken);
            }
        }
    }
}
