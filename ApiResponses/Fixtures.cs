
using System.Collections.Generic;

namespace MyResumeSiteModels.ApiResponses
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
        public StandingsForCurrentTeam standings { get; init; }
        public bool? is_placeholder { get; init; }
        public Team localTeam { get; init; }
        public Team visitorTeam { get; init; }
        public Venue venue { get; init; }
        public League league { get; set; }

        public Substitutions substitutions { get; init; }
        public Goals goals { get; init; }
        public Cards cards { get; init; }
        public Corners corners { get; init; }
        public Stats stats { get; init; }
        public Events events { get; init; }
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


    public record StandingsForCurrentTeam
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

    // Live Data



    public class Substitutions
    {
        public List<SubstituteData> Data { get; set; }
    }

    public class SubstituteData
    {
        public double? Id { get; set; }
        public string team_id { get; set; }
        public string type { get; set; }
        public int? fixture_id { get; set; }
        public int? player_in_id { get; set; }
        public string player_in_name { get; set; }
        public int? player_out_id { get; set; }
        public string player_out_name { get; set; }
        public int? minute { get; set; }
        public object extra_minute { get; set; }
        public bool? injuried { get; set; }
    }

    public class Goals
    {
        public GoalsData[] data { get; set; }
    }

    public class GoalsData
    {
        public double? id { get; set; }
        public string team_id { get; set; }
        public string type { get; set; }
        public int? fixture_id { get; set; }
        public int? player_id { get; set; }
        public string player_name { get; set; }
        public int? player_assist_id { get; set; }
        public string player_assist_name { get; set; }
        public int? minute { get; set; }
        public object extra_minute { get; set; }
        public object reason { get; set; }
        public string result { get; set; }
    }

    public class Cards
    {
        public CardsData[] data { get; set; }
    }

    public class CardsData
    {
        public double? id { get; set; }
        public string team_id { get; set; }
        public string type { get; set; }
        public int? fixture_id { get; set; }
        public int? player_id { get; set; }
        public string player_name { get; set; }
        public int? minute { get; set; }
        public object extra_minute { get; set; }
        public object reason { get; set; }
        public object on_pitch { get; set; }
    }

    public class Corners
    {
        public CornersData[] data { get; set; }
    }

    public class CornersData
    {
        public double? id { get; set; }
        public int? team_id { get; set; }
        public int? fixture_id { get; set; }
        public int? minute { get; set; }
        public object extra_minute { get; set; }
        public string comment { get; set; }
    }

    public class Events
    {
        public EventsData[] data { get; set; }
    }

    public class EventsData
    {
        public double? id { get; set; }
        public string team_id { get; set; }
        public string type { get; set; }
        public string var_result { get; set; }
        public int? fixture_id { get; set; }
        public int? player_id { get; set; }
        public string player_name { get; set; }
        public int? related_player_id { get; set; }
        public string related_player_name { get; set; }
        public int? minute { get; set; }
        public int? extra_minute { get; set; }
        public string reason { get; set; }
        public bool? injuried { get; set; }
        public string result { get; set; }
        public bool? on_pitch { get; set; }
    }
    public class Stats
    {
        public StartsData[] data { get; set; }
    }

    public class StartsData
    {
        public int? team_id { get; set; }
        public int? fixture_id { get; set; }
        public Shots shots { get; set; }
        public Passes passes { get; set; }
        public Attacks attacks { get; set; }
        public int? fouls { get; set; }
        public int? corners { get; set; }
        public int? offsides { get; set; }
        public int? possessiontime { get; set; }
        public int? yellowcards { get; set; }
        public int? redcards { get; set; }
        public int? yellowredcards { get; set; }
        public int? saves { get; set; }
        public int? substitutions { get; set; }
        public int? goal_kick { get; set; }
        public int? goal_attempts { get; set; }
        public int? free_kick { get; set; }
        public int? throw_in { get; set; }
        public int? ball_safe { get; set; }
        public int? goals { get; set; }
        public int? penalties { get; set; }
        public int? injuries { get; set; }
        public int? tackles { get; set; }
    }

    public class Shots
    {
        public int? total { get; set; }
        public int? ongoal { get; set; }
        public int? offgoal { get; set; }
        public int? blocked { get; set; }
        public int? insidebox { get; set; }
        public int? outsidebox { get; set; }
    }

    public class Passes
    {
        public int? total { get; set; }
        public int? accurate { get; set; }
        public float? percentage { get; set; }
    }

    public class Attacks
    {
        public int? attacks { get; set; }
        public int? dangerous_attacks { get; set; }
    }

}
