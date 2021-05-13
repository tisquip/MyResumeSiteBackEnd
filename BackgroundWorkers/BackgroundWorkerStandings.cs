using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyResumeSiteBackEnd.Hubs;

using MyResumeSiteBackEnd.Models.ApiResponses;

namespace MyResumeSiteBackEnd.BackgroundWorkers
{
    public class BackgroundWorkerStandings : IHostedService
    {
        public static List<Exception> Last10Exceptions = new List<Exception>();
        Timer _timer;
        private readonly ILogger<BackgroundWorkerStandings> _logger;
        private readonly HttpClient _httpClient;
        private readonly IHubContext<MessageHub> _hubContext;
        string _apiKey = "";

        public BackgroundWorkerStandings(ILogger<BackgroundWorkerStandings> logger, IConfiguration configuration, HttpClient httpClient, IHubContext<MessageHub> hubContext)
        {
            _logger = logger;
            _httpClient = httpClient;
            _hubContext = hubContext;
            _apiKey = Helpers.ApiKeyGetApiVariables(configuration);
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
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
