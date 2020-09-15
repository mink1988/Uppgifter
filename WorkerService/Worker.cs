using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerServiceVG
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private string _url = "http://api.openweathermap.org/data/2.5/weather?q=orebro&units=metric&appid=a627a0c523b7b9134ce9b828fcd748f3";
        private HttpClient _client;
        private HttpResponseMessage _result;
        //await _result.Content.ReadAsStringAsync();
        /*string json = @"{
  'Name': 'Bad Boys',
  'ReleaseDate': '1995-4-7T00:00:00',
  'Genres': [
    'Action',
    'Comedy'
  ]
    }";

Movie m = JsonConvert.DeserializeObject<Movie>(json);

    string name = m.Name;
    // Bad Boys
        xxxxzzzzzzssxxzzaa
        */

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
                await Task.Delay(60 * 1000, stoppingToken);
            }
        }
    }
}
