namespace MyResumeSiteModels.ApiResponses
{
    public record FixturesWithLeagues
    {
        public Leagues Leagues { get; set; }
        public Fixtures Fixtures { get; set; }
    }
}
