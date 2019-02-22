using System;

namespace CSharpWars.Common.Configuration.Interfaces
{
    public interface IConfigurationHelper
    {
        String ConnectionString { get; set; }
        Int32 ArenaSize { get; set; }
    }
}