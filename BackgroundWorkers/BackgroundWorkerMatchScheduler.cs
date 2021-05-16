using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MyResumeSiteBackEnd.Exceptions;
using MyResumeSiteBackEnd.Hubs;
using MyResumeSiteBackEnd.Services;

using MyResumeSiteModels;
using MyResumeSiteModels.ApiResponses;

namespace MyResumeSiteBackEnd.BackgroundWorkers
{
    public class BackgroundWorkerMatchScheduler : IHostedService
    {
        public static List<Exception> Last10Exceptions = new List<Exception>();
        public static Leagues Leagues { get; set; }
        public static Fixtures Fixtures { get; set; }
        private readonly ILogger<BackgroundWorkerMatchScheduler> _logger;
        Timer _timer;
        HttpClient _httpClient;
        private readonly IHubContext<MessageHub> _hubContext;
        private readonly IEmailService _emailService;
        private string _apiKey;

        public BackgroundWorkerMatchScheduler(ILogger<BackgroundWorkerMatchScheduler> logger, IConfiguration configuration, HttpClient httpClient, IHubContext<MessageHub> hubContext, IEmailService emailService)
        {
            _logger = logger;
            _httpClient = httpClient;
            _hubContext = hubContext;
            _emailService = emailService;
            _apiKey = Helpers.ApiKeyGetApiVariables(configuration);
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Process, cancellationToken, TimeSpan.FromSeconds(0), TimeSpan.FromHours(24));
            if (!VariablesCore.ServerUrl.Contains("local"))
            {
                await _emailService.SendEmail("tisquip6@gmail.com", "Background Service Started", $"BackgroundWorkerMatchScheduler started on {VariablesCore.ServerUrl}");
            }
            
        }
        bool isProcessing = false;

        async void Process(object state)
        {
            if (!isProcessing)
            {
                isProcessing = true;
                try
                {
                    AddEvent("Processing Started");

                    Leagues = await _httpClient.GetFromJsonAsync<Leagues>($"{Variables.SportsMonkApiBaseUrl}leagues{Variables.GetApiKeyUrlFormatted(_apiKey)}");

                    DateTime startDate = DateTime.UtcNow;
                    DateTime endDate = DateTime.UtcNow.AddDays(7);


                    string url = $"{Variables.SportsMonkApiBaseUrl}fixtures/between/{startDate.ToStringSportsMonkFormatting()}/{endDate.ToStringSportsMonkFormatting()}{Variables.GetApiKeyUrlFormatted(_apiKey)}&include=localTeam,visitorTeam,venue";

                    AddEvent("Processing Started with", new Dictionary<string, object>()
                    {
                        {"Start Date:", startDate },
                        {"End Date:", endDate },
                        {"Leagues:", Leagues },
                        {"Url", url }
                    });

                    Fixtures = await _httpClient.GetFromJsonAsync<Fixtures>(url);
                    await _hubContext.Clients.All.SendAsync(VariablesCore.SignalRMethodNameFixturesUpdated);
                    AddEvent("Fixtures sent", Fixtures);
                }
                catch (Exception ex)
                {
                    await HandleException(ex);
                }
                isProcessing = false;
            }
        }

        private async Task HandleException(Exception ex)
        {
            if (!VariablesCore.ServerUrl.Contains("local"))
            {
                await _emailService.SendAsyncToAdminException(ex);
            }
            AddEvent("Exception", ex);
            _logger.LogError(ex, "Error occured {@ex}", ex);
            Last10Exceptions.Insert(0, ex);
            int exceptionsCount = Last10Exceptions.Count;
            if (exceptionsCount > 10)
            {
                Last10Exceptions.RemoveAt(exceptionsCount - 1);
            }

            
        }

        public static List<string> Last50Events = new List<string>();
        void AddEvent(string evnt)
        {
            Last50Events.Insert(0, $"{DateTime.UtcNow}| BackgroundWorkerMatchScheduler | {evnt}");
            if (Last50Events.Count > 50)
            {
                Last50Events.RemoveAt(Last50Events.Count - 1);
            }
        }

        void AddEvent(string evnt, object data)
        {
            if (data != null)
            {
                try
                {
                    evnt += $" | Data: {Newtonsoft.Json.JsonConvert.SerializeObject(data)}";
                }
                catch (Exception)
                {
                }
            }
            else
            {
                evnt += $" | Data: was null";
            }

            AddEvent(evnt);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await HandleException(new BackgroundWorkerStoppedException(typeof(BackgroundWorkerMatchScheduler)));

            _timer?.Dispose();
        }
    }
}
