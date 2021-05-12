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
    public class BackgroundWorkerMatchScheduler : IHostedService
    {
        public static List<Exception> Last10Exceptions = new List<Exception>();

        private readonly ILogger<BackgroundWorkerMatchScheduler> _logger;
        Timer _timer;
        HttpClient _httpClient;
        private string _apiKey;

        public BackgroundWorkerMatchScheduler(ILogger<BackgroundWorkerMatchScheduler> logger, IConfiguration configuration, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _apiKey = Helpers.ApiKeyGetApiVariables(configuration);
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Process, cancellationToken, TimeSpan.FromSeconds(15), TimeSpan.FromHours(24));
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
                    int exceptionsCount = Last10Exceptions.Count;
                    if (exceptionsCount > 10)
                    {
                        Last10Exceptions.RemoveAt(exceptionsCount - 1);
                    }
                }
                isProcessing = false;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return Task.CompletedTask;
        }
    }
}
