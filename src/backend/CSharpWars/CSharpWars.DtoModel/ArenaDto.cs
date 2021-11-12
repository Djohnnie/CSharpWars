namespace CSharpWars.DtoModel;

public record ArenaDto(int Width, int Height)
{
    public static ArenaDto Empty => new ArenaDto(0, 0);
}