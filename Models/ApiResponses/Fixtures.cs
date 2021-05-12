namespace MyResumeSiteBackEnd.Models.ApiResponses
{
    public class Fixtures
    {
        public FixturesData[] data { get; set; }
    }

    public class FixturesData
    {
        public int? id { get; set; }
        public int? league_id { get; set; }
        public int? season_id { get; set; }
        public Scores scores { get; set; }
        public Time time { get; set; }
        public Standings standings { get; set; }
        public bool? is_placeholder { get; set; }
        public Team localTeam { get; set; }
        public Team visitorTeam { get; set; }
        public Venue venue { get; set; }
    }

    public class Scores
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

    public class Time
    {
        public string Status { get; init; }
        public Starting_At Starting_At { get; init; }
        public double? Minute { get; init; }
        public double? Second { get; init; }
        public double? Added_Time { get; init; }
        public double? Extra_Minute { get; init; }
        public double? Injury_Time { get; init; }
    }

    public class Starting_At
    {
        public string Date_Time { get; init; }
        public string Date { get; init; }
        public string Time { get; init; }
        public double? Timestamp { get; init; }
        public string Timezone { get; init; }
    }


    public class Standings
    {
        public int? localteam_position { get; set; }
        public int? visitorteam_position { get; set; }
    }


    public class Team
    {
        public TeamData data { get; set; }
    }

    public class TeamData
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


    public class Venue
    {
        public VenuData data { get; set; }
    }

    public class VenuData
    {
        public int? id { get; set; }
        public string name { get; set; }
        public string surface { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public int? capacity { get; set; }
        public string image_path { get; set; }
        public string coordinates { get; set; }
    }



}
