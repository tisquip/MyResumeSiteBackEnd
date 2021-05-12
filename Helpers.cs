
using System;
using System.Net.Http;

using Microsoft.Extensions.Configuration;

namespace MyResumeSiteBackEnd
{
    public static class Helpers
    {
        public static string ApiKeyGetApiVariables(IConfiguration configuration)
        {
            return configuration.GetValue<string>("SportsMonkKey");
        }
    }
}
