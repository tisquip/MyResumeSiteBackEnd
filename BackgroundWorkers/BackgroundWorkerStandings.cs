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
using MyResumeSiteBackEnd.Services;

using MyResumeSiteModels;
using MyResumeSiteModels.ApiResponses;

namespace MyResumeSiteBackEnd.BackgroundWorkers
{
    public class BackgroundWorkerStandings : IHostedService
    {
        public static List<Exception> Last10Exceptions = new List<Exception>();
        Timer _timerMain;
        public static List<StandingsApiResponseWithLeagueData> StandingsApiResponsesWithLeague = new List<StandingsApiResponseWithLeagueData>();
        private readonly ILogger<BackgroundWorkerStandings> _logger;
        private readonly HttpClient _httpClient;
        private readonly IHubContext<MessageHub> _hubContext;
        private readonly IEmailService _emailService;
        string _apiKey = "";


        public BackgroundWorkerStandings(ILogger<BackgroundWorkerStandings> logger, IConfiguration configuration, HttpClient httpClient, IHubContext<MessageHub> hubContext, IEmailService emailService)
        {
            _logger = logger;
            _httpClient = httpClient;
            _hubContext = hubContext;
            _emailService = emailService;
            _apiKey = Helpers.ApiKeyGetApiVariables(configuration);
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _timerMain = new Timer(Process, cancellationToken, TimeSpan.FromMinutes(1), TimeSpan.FromHours(1));

            if (!VariablesCore.ServerUrl.Contains("local"))
            {
                await _emailService.SendEmail("tisquip6@gmail.com", "Background Service Started", $"BackgroundWorkerStandings started on {VariablesCore.ServerUrl}");
            }
        }

        bool isProcessing = false;
        async void Process(object state)
        {
            if (BackgroundWorkerMatchScheduler.Leagues?.data?.Any() ?? false)
            {
                if (!isProcessing)
                {
                    isProcessing = true;
                    AddEvent("Processing Started with Leagues", BackgroundWorkerMatchScheduler.Leagues?.data);
                    try
                    {
                        StandingsApiResponsesWithLeague = new List<StandingsApiResponseWithLeagueData>();
                        string urlLeaguesBase = "https://soccer.sportmonks.com/api/v2.0/standings/season/";
                        foreach (var league in BackgroundWorkerMatchScheduler.Leagues.data)
                        {
                            try
                            {
                                if (league.is_cup.Value)
                                {
                                    continue;
                                }

                                string urlToUse = $"{urlLeaguesBase}{league.current_season_id}{Variables.GetApiKeyUrlFormatted(_apiKey)}";

                                var response = await _httpClient.GetAsync(urlToUse);
                                if (response.IsSuccessStatusCode)
                                {
                                    string content = await response.Content.ReadAsStringAsync();
                                    StandingsApiResponse standingsApiResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<StandingsApiResponse>(content);

                                    StandingsApiResponsesWithLeague
                                        .Add(new StandingsApiResponseWithLeagueData() 
                                        { LeagueData = league, StandingsApiResponse = standingsApiResponse });
                                }
                            }
                            catch (Exception ex)
                            {
                                await HandleError(ex);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                       await HandleError(ex);
                    }


                    if (StandingsApiResponsesWithLeague?.Any() ?? false)
                    {
                        var standingsWithoutData = StandingsApiResponsesWithLeague.Where(s => !s.StandingsApiResponse?.data?.Any() ?? true)?.ToList() ?? new List<StandingsApiResponseWithLeagueData>();

                        if (standingsWithoutData?.Any() ?? false)
                        {
                            foreach (var itemToRemove in standingsWithoutData)
                            {
                                StandingsApiResponsesWithLeague.Remove(itemToRemove);
                            }
                        }

                        if (StandingsApiResponsesWithLeague.Any())
                        {
                            await _hubContext.Clients.All.SendAsync(VariablesCore.SignalRMethodNameStandings);
                            AddEvent("League signalR update sent");
                        }
                    }
                    AddEvent("Processing fininished. Results are", StandingsApiResponsesWithLeague);
                    isProcessing = false;
                }
            }
            else
            {
                AddEvent("No League Data was found for processing to start");
            }
           
        }

        public static List<string> Last50Events = new List<string>();

        void AddEvent(string evnt)
        {
            Last50Events.Insert(0, $"{DateTime.UtcNow}| BackgroundWorkerStandings | {evnt}");
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

        async Task HandleError(Exception ex)
        {
            _logger.LogError(ex, "Error occured {@ex}", ex);

            Last10Exceptions.Insert(0, ex);
            int count = Last10Exceptions.Count;
            if (count > 10)
            {
                Last10Exceptions.RemoveAt(count - 1);
            }

            AddEvent("Exception", ex);
            if (!VariablesCore.ServerUrl.Contains("local"))
            {
                await _emailService.SendAsyncToAdminException(ex);
            }
            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timerMain?.Dispose();
            return Task.CompletedTask;
        }

    }
}
