namespace MyResumeSiteModels.ApiResponses
{
    public record Leagues
    {
        public LeaguesData[] data { get; init; }
    }

    public record LeaguesData
    {
        public int? id { get; init; }
        public bool active { get; init; }
        public int? country_id { get; init; }
        public string logo_path { get; init; }
        public string name { get; init; }
        public bool? is_cup { get; init; }
        public int? current_season_id { get; init; }
    }

    public record League
    {
        public LeaguesData data { get; set; }
    }

}
