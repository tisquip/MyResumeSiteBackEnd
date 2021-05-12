namespace MyResumeSiteBackEnd.Models.ApiResponses
{
    public class Leagues
    {
        public LeaguesData[] data { get; set; }
    }

    public class LeaguesData
    {
        public int? id { get; set; }
        public bool active { get; set; }
        public int? country_id { get; set; }
        public string logo_path { get; set; }
        public string name { get; set; }
        public bool? is_cup { get; set; }
        public int? current_season_id { get; set; }
    }
}
