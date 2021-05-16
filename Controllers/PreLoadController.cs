using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using MyResumeSiteBackEnd.BackgroundWorkers;

using MyResumeSiteModels.ApiResponses;

namespace MyResumeSiteBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreLoadController : ControllerBase
    {
        [HttpGet("Standings")]
        public ActionResult<List<StandingsApiResponseWithLeagueData>> Standings()
        {
            if (BackgroundWorkerStandings.StandingsApiResponsesWithLeague != null)
            {
                return Ok(BackgroundWorkerStandings.StandingsApiResponsesWithLeague);
            }
            return NotFound("Data from third party api was unavailable");

        }

        [HttpGet("LiveMatches")]
        public ActionResult<Fixtures> LiveMatches()
        {
            if (BackgroundMatchBroadcaster.FixturesLive != null)
            {
                return Ok(BackgroundMatchBroadcaster.FixturesLive);
            }
            return NotFound("Data from third party api was unavailable");
        }

        [HttpGet("Schedule")]
        public ActionResult<FixturesWithLeagues> Schedule()
        {
            if (BackgroundWorkerMatchScheduler.Fixtures != null)
            {
                FixturesWithLeagues fixturesWithLeagues = new FixturesWithLeagues()
                {
                    Leagues = BackgroundWorkerMatchScheduler.Leagues,
                    Fixtures = BackgroundWorkerMatchScheduler.Fixtures
                };
                return Ok(fixturesWithLeagues);
            }
            return NotFound("Data from third party api was unavailable");
        }
    }
}
