namespace MyResumeSiteBackEnd.Models.ApiResponses
{
    public record Fixtures
    {
        public FixturesData[] data { get; init; }
    }

    public record FixturesData
    {
        public int? id { get; init; }
        public int? league_id { get; init; }
        public int? season_id { get; init; }
        public Scores scores { get; init; }
        public Time time { get; init; }
        public Standings standings { get; init; }
        public bool? is_placeholder { get; init; }
        public Team localTeam { get; init; }
        public Team visitorTeam { get; init; }
        public Venue venue { get; init; }
    }

    public record Scores
    {
        public double? Localteam_Score { get; init; }
        public double? Visitorteam_Score { get; init; }
        public double? Localteam_Pen_Score { get; init; }
        public double? Visitorteam_Pen_Score { get; init; }
        public string Ht_Score { get; init; }
        public string Ft_Score { get; init; }
        public string Et_Score { get; init; }
        public string Ps_Score { get; init; }
    }

    public record Time
    {
        public string Status { get; init; }
        public Starting_At Starting_At { get; init; }
        public double? Minute { get; init; }
        public double? Second { get; init; }
        public double? Added_Time { get; init; }
        public double? Extra_Minute { get; init; }
        public double? Injury_Time { get; init; }
    }

    public record Starting_At
    {
        public string Date_Time { get; init; }
        public string Date { get; init; }
        public string Time { get; init; }
        public double? Timestamp { get; init; }
        public string Timezone { get; init; }
    }


    public record Standings
    {
        public int? localteam_position { get; init; }
        public int? visitorteam_position { get; init; }
    }


    public record Team
    {
        public TeamData data { get; init; }
    }

    public record TeamData
    {
        public double? Id { get; init; }
        public string Name { get; init; }
        public string Short_Code { get; init; }
        public string Twitter { get; init; }
        public double? Country_Id { get; init; }
        public double? Founded { get; init; }
        public string Logo_Path { get; init; }
        public double? Venue_Id { get; init; }
        public double? Current_Season_Id { get; init; }
        public bool? Is_Placeholder { get; init; }
    }


    public record Venue
    {
        public VenuData data { get; init; }
    }

    public record VenuData
    {
        public int? id { get; init; }
        public string name { get; init; }
        public string surface { get; init; }
        public string address { get; init; }
        public string city { get; init; }
        public int? capacity { get; init; }
        public string image_path { get; init; }
        public string coordinates { get; init; }
    }



}
