using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyResumeSiteModels.ApiResponses
{
    public class StandingsApiResponse
    {
        public StandingsApiResponseData[] data { get; set; }
    }

    public class StandingsApiResponseData
    {
        public int? id { get; set; }
        public string name { get; set; }
        public int? league_id { get; set; }
        public int? season_id { get; set; }
        public int? round_id { get; set; }
        public int? round_name { get; set; }
        public string type { get; set; }
        public int? stage_id { get; set; }
        public string stage_name { get; set; }
        public string resource { get; set; }
        public Standings standings { get; set; }
    }

    public class Standings
    {
        public StandingsData[] data { get; set; }
    }

    public class StandingsData
    {
        public int? position { get; set; }
        public int? team_id { get; set; }
        public string team_name { get; set; }
        public int? round_id { get; set; }
        public int? round_name { get; set; }
        public object group_id { get; set; }
        public object group_name { get; set; }
        public Overall overall { get; set; }
        public Home home { get; set; }
        public Away away { get; set; }
        public Total total { get; set; }
        public string result { get; set; }
        public int? points { get; set; }
        public string recent_form { get; set; }
        public object status { get; set; }
    }

    public class Overall
    {
        public int? games_played { get; set; }
        public int? won { get; set; }
        public int? draw { get; set; }
        public int? lost { get; set; }
        public int? goals_scored { get; set; }
        public int? goals_against { get; set; }
    }

    public class Home
    {
        public int? games_played { get; set; }
        public int? won { get; set; }
        public int? draw { get; set; }
        public int? lost { get; set; }
        public int? goals_scored { get; set; }
        public int? goals_against { get; set; }
    }

    public class Away
    {
        public int? games_played { get; set; }
        public int? won { get; set; }
        public int? draw { get; set; }
        public int? lost { get; set; }
        public int? goals_scored { get; set; }
        public int? goals_against { get; set; }
    }

    public class Total
    {
        public string goal_difference { get; set; }
        public int? points { get; set; }
    }

}
