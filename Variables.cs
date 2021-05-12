using System.Linq;

namespace MyResumeSiteBackEnd
{
    public static class Variables
    {
        public static string SportsMonkApiBaseUrl => "https://soccer.sportmonks.com/api/v2.0/";
        public static string SportsMonkApiUrlAddApiKey(string url, string apiKey, bool useApmersand = false)
        {
            return $"{url}{GetApiKeyUrlFormatted(apiKey, useApmersand)}";
        }

        public static string GetApiKeyUrlFormatted(string apiKey, bool useApmersand = false)
        {
            return $"{(useApmersand ? "&" : "?")}api_token={apiKey}";
        }

        public static string MessageHubUrlEndPointWithPreSlash => "/messages";
        public static string SignalRMethodNameFixturesUpdated => "RealTimeFixturesChanged";
        public static string SignalRMethodNameLiveMatch => "RealTimeLiveMatch";
        public static string AppUrl => "https://localhost:44371/";


    }
}
