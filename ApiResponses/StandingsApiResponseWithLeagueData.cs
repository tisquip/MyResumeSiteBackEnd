namespace MyResumeSiteModels.ApiResponses
{
    public record StandingsApiResponseWithLeagueData
    {
        public StandingsApiResponse StandingsApiResponse { get; init; }
        public LeaguesData LeagueData { get; init; }
    }
}
