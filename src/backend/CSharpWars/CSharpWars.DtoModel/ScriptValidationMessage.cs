namespace CSharpWars.DtoModel;

public record ScriptValidationMessage(string Message, int LocationStart, int LocationEnd);