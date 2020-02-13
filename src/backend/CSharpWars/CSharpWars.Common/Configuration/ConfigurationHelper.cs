using System;
using CSharpWars.Common.Configuration.Interfaces;

namespace CSharpWars.Common.Configuration
{
    public class ConfigurationHelper : IConfigurationHelper
    {
        public string ConnectionString { get; set; }
        public int ArenaSize { get; set; }
        public string ValidationHost { get; set; }
        public int PointsLimit { get; set; }
        public int BotDeploymentLimit { get; set; }
    }
}