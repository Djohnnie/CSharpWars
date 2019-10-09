using System;

namespace CSharpWars.Common.Configuration.Interfaces
{
    public interface IConfigurationHelper
    {
        String ConnectionString { get; set; }
        Int32 ArenaSize { get; set; }
        String ValidationHost { get; set; }
        Int32 PointsLimit { get; set; }
        Int32 BotDeploymentLimit { get; set; }
    }
}