namespace CSharpWars.DtoModel;

public record BotToCreateDto(Guid PlayerId, string Name, int MaximumHealth, int MaximumStamina, string Script);