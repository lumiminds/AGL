using System;
using Microsoft.Extensions.Hosting;

namespace AGL.App.Extensions
{
    public static class EnvironmentExtensions
    {
        // This is the Virtualised "Local" environment, instead of the local pc `ASPNETCOREENVIRONMENT=Development`
        public static bool IsLocal(this IHostEnvironment env)
        {
            return env.IsEnvironment(EnvironmentLabels.Local);
        }
    }

    public static class EnvironmentLabels
    {
        public const string Local = "Local";
    }
}
