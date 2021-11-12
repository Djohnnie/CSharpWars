using CSharpWars.Logic.Interfaces;
using CSharpWars.Common.Configuration.Interfaces;
using CSharpWars.DtoModel;

namespace CSharpWars.Logic;

public class ArenaLogic : IArenaLogic
{
    private readonly IConfigurationHelper _configurationHelper;

    public ArenaLogic(IConfigurationHelper configurationHelper)
    {
        _configurationHelper = configurationHelper;
    }
    public Task<ArenaDto> GetArena()
    {
        return Task.FromResult(new ArenaDto(_configurationHelper.ArenaSize, _configurationHelper.ArenaSize));
    }
}