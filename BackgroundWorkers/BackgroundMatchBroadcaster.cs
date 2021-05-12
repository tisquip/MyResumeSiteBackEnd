using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
    public class BackgroundMatchBroadcaster : IHostedService
    {
        public static List<Exception> Last10Exceptions = new List<Exception>();
        Timer _timer;
        private readonly ILogger<BackgroundMatchBroadcaster> _logger;
        private readonly HttpClient _httpClient;
        private readonly IHubContext<MessageHub> _hubContext;
        string _apiKey = "";

        public BackgroundMatchBroadcaster(ILogger<BackgroundMatchBroadcaster> logger, IConfiguration configuration, HttpClient httpClient, IHubContext<MessageHub> hubContext)
        {
            _logger = logger;
            _httpClient = httpClient;
            _hubContext = hubContext;
            _apiKey = Helpers.ApiKeyGetApiVariables(configuration);
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            SetTimer(null);
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
                    string urlFixturesNow = $"https://soccer.sportmonks.com/api/v2.0/livescores/now?leagues=&include=localTeam,visitorTeam,substitutions,goals,cards,events,corners,stats{Variables.GetApiKeyUrlFormatted(_apiKey, true)}";

                    string urlFixturesOfTheDay = $"https://soccer.sportmonks.com/api/v2.0/livescores{Variables.GetApiKeyUrlFormatted(_apiKey)}";

                    Fixtures fixtures = await _httpClient.GetFromJsonAsync<Fixtures>(urlFixturesNow);
                    if (!fixtures?.data?.Any() ?? true)
                    {
                        fixtures = await _httpClient.GetFromJsonAsync<Fixtures>(urlFixturesOfTheDay);
                        SetTimer(fixtures);
                    }
                    else
                    {
                        SetTimerToBroadCastTime();
                        string fixturesJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(fixtures);
                        await _hubContext.Clients.All.SendAsync(Variables.SignalRMethodNameLiveMatch, fixturesJsonString);
                    }
                    
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

        private void SetTimerToBroadCastTime()
        {
            if (!_isOnBroadcastTimer)
            {
                _isOnBroadcastTimer = true;
                _timer.Change(TimeSpan.FromSeconds(15), _broadcastTimer);
            }
        }

        bool _isOnBroadcastTimer = false;
        TimeSpan _broadcastTimer = TimeSpan.FromSeconds(30);
        TimeSpan _restTimer = TimeSpan.FromHours(11);
        private void SetTimer(Fixtures fixtures)
        {
            TimeSpan amountOfTimeBeforeNextCall = _broadcastTimer;
            _isOnBroadcastTimer = false;
            if (_timer == null)
            {
                _timer = new Timer(Process, null, TimeSpan.FromSeconds(15), amountOfTimeBeforeNextCall);
                _isOnBroadcastTimer = true;
            }
            else
            {
                if (!fixtures?.data?.Any() ?? true)
                {
                    amountOfTimeBeforeNextCall = _restTimer;
                }
                else
                {
                    List<FixturesData> fixturesData = fixtures.data
                        .Where(f => DateTime.Parse(f.time.Starting_At.Date_Time) > DateTime.UtcNow.AddMinutes(-5) )
                        .OrderBy(f => DateTime.Parse(f.time.Starting_At.Date_Time)).ToList();
                    
                    var nextFixture = fixturesData?.FirstOrDefault();

                    if (nextFixture == null)
                    {
                        amountOfTimeBeforeNextCall = _restTimer;
                    }
                    else
                    {
                        DateTime timeToNextFixture = DateTime.Parse(nextFixture.time.Starting_At.Date_Time);
                        amountOfTimeBeforeNextCall = timeToNextFixture - DateTime.UtcNow;
                        if (amountOfTimeBeforeNextCall.TotalMinutes < 3)
                        {
                            amountOfTimeBeforeNextCall = _broadcastTimer;
                            _isOnBroadcastTimer = true;
                        }
                    }
                }
                _timer.Change(TimeSpan.FromSeconds(15), amountOfTimeBeforeNextCall);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return Task.CompletedTask;
        }

    }
}
