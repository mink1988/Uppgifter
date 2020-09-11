using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private string _url = "https://gais.se";
        private HttpClient _client;
        private HttpResponseMessage _result;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _client = new HttpClient();
            _logger.LogInformation("The service has been started");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _client.Dispose();
            _logger.LogInformation("The service has been stopped");
            return base.StopAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {

                    _result = await _client.GetAsync(_url);

                    if (_result.IsSuccessStatusCode)
                        _logger.LogInformation($"The website ({_url}) is up. Status Code = {_result.StatusCode}");
                    else _logger.LogInformation($"The website ({_url}) is down. Status Code = {_result.StatusCode}");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Failed. The webbsite ({_url}) - {ex.Message}");
                }
                await Task.Delay(5 * 1000, stoppingToken);
            }
        }
    }
}
