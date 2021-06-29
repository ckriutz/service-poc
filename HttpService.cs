using System;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Http;


namespace service_poc
{
    public class HttpService : IHostedService, IDisposable
    {
        private readonly ILogger<HttpService> _logger;
        private readonly HttpClient _client;
        private Timer _timer;

        public HttpService(ILogger<HttpService> logger, HttpClient client)
        {
            _logger = logger;
            _client = client;
            _client.DefaultRequestHeaders.Add("User-Agent", "ckriutz console.");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("HttpService is starting.");

            // This tells us to run the DoWork method every 30 seconds.
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            try
            {
                //HttpResponseMessage response = await _client.GetAsync("https://api.github.com/zen");
                //response.EnsureSuccessStatusCode();
                //string responseBody = await response.Content.ReadAsStringAsync();
                string responseBody = await _client.GetStringAsync("https://api.github.com/zen");
                _logger.LogInformation(responseBody);
            }
            catch(Exception ex)
            {
                _logger.LogError("Things have happened. :( " + ex);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}