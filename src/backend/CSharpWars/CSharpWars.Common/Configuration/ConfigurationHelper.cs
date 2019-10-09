using System;
using CSharpWars.Common.Configuration.Interfaces;

namespace CSharpWars.Common.Configuration
{
    public class ConfigurationHelper : IConfigurationHelper
    {
        public String ConnectionString { get; set; }
        public Int32 ArenaSize { get; set; }
        public String ValidationHost { get; set; }
        public Int32 PointsLimit { get; set; }
        public Int32 BotDeploymentLimit { get; set; }
    }
}