using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyResumeSiteBackEnd.BackgroundWorkers
{
    public class BackgroundMatchBroadcaster : IHostedService
    {
        public static List<Exception> Last10Exceptions = new List<Exception>();
        Timer _timer;
        private readonly ILogger<BackgroundMatchBroadcaster> _logger;
        private readonly HttpClient _httpClient;
        string _apiKey = "";

        public BackgroundMatchBroadcaster(ILogger<BackgroundMatchBroadcaster> logger, IConfiguration configuration, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _apiKey = Helpers.ApiKeyGetApiVariables(configuration);
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Process, cancellationToken, TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(30));
            return Task.CompletedTask;
        }

        bool isProcessing = false;
        async void Process(object state)
        {
            if (!isProcessing)
            {
                isProcessing = true;
                try
                {

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occured {@ex}", ex);

                    Last10Exceptions.Insert(0, ex);
                    int count = Last10Exceptions.Count;
                    if (count > 10)
                    {
                        Last10Exceptions.RemoveAt(count - 1);
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return Task.CompletedTask;
        }

    }
}
