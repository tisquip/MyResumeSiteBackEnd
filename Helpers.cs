
using Microsoft.Extensions.Configuration;

namespace MyResumeSiteBackEnd
{
    public static class Helpers
    {
        public static (string ApiKey, int TeamId, int LeagueId) GetApiVariable(IConfiguration configuration)
        {
            (string ApiKey, int TeamId, int LeagueId) vtr = ("", 0, 0);
            vtr.ApiKey = configuration.GetValue<string>("ApiFootballKey");
            vtr.TeamId = configuration.GetValue<int>("TeamId");
            vtr.LeagueId = configuration.GetValue<int>("LeagueId");

            return vtr;
        }
    }
}
