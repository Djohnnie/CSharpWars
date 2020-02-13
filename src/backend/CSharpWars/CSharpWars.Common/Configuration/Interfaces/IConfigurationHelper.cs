using System;

namespace CSharpWars.Common.Configuration.Interfaces
{
    public interface IConfigurationHelper
    {
        string ConnectionString { get; set; }
        int ArenaSize { get; set; }
        string ValidationHost { get; set; }
        int PointsLimit { get; set; }
        int BotDeploymentLimit { get; set; }
    }
}