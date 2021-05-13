using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

using MyResumeSiteModels;
using MyResumeSiteModels.ApiResponses;

namespace MyResumeSiteBackEnd.Services
{
    public class SignalRService
    {
        private readonly NotificationService _notificationService;

        public HubConnection HubConnection { get; private set; }

        public event EventHandler FixturesUpdates;
        protected virtual void OnFixturesUpdates()
        {
            EventHandler handler = FixturesUpdates;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public event EventHandler<Fixtures> LiveMatchUpdate;

        protected virtual void OnLiveMatchUpdate(string fixturesJson)
        {
            try
            {
                if (LiveMatchUpdate != null)
                {
                    Fixtures vts = Newtonsoft.Json.JsonConvert.DeserializeObject<Fixtures>(fixturesJson);
                    if (vts != null)
                    {
                        LiveMatchUpdate.Invoke(this, vts);
                    }
                }
            }
            catch (Exception ex)
            {
                ;
            }
        }

        public SignalRService(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        static bool isAttemptingToConnect = false;
        public async Task ConnectIfNecessary()
        {
            if (!isAttemptingToConnect && (HubConnection == null || HubConnection.State == HubConnectionState.Disconnected))
            {
                isAttemptingToConnect = true;
                try
                {
                    string hubEndpoint = VariablesCore.MessageHubUrlEndPointWithPreSlash;
                    hubEndpoint = hubEndpoint.Replace("/", "");
                    string url = $"{VariablesCore.ServerUrl}{hubEndpoint}";
                    
                    HubConnection = new HubConnectionBuilder()
                        .WithUrl(url)
                        .WithAutomaticReconnect()
                        .Build();

                    HubConnection.On(VariablesCore.SignalRMethodNameFixturesUpdated, () =>
                    {
                        OnFixturesUpdates();
                    });

                    HubConnection.On<string>(VariablesCore.SignalRMethodNameLiveMatch, (fixturesJson) =>
                    {
                        OnLiveMatchUpdate(fixturesJson);
                    });

                    await HubConnection.StartAsync();
                }
                catch (Exception ex)
                {
                    string m = ex.Message;
                    throw;
                }
               
                await _notificationService.Notify($"Real Time Connection State : {HubConnection.State}");
                isAttemptingToConnect = false;
            }
        }
    }
}
