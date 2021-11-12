namespace CSharpWars.DtoModel;

public record ValidatedScriptDto(
    string Script,
    long CompilationTimeInMilliseconds,
    long RunTimeInMilliseconds,
    List<ScriptValidationMessage> Messages);