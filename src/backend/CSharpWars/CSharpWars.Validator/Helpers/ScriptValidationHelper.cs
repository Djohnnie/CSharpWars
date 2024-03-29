﻿using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using CSharpWars.Common.Extensions;
using CSharpWars.Common.Tools;
using CSharpWars.DtoModel;
using CSharpWars.Enums;
using CSharpWars.Scripting;
using CSharpWars.Scripting.Model;
using CSharpWars.Validator.Helpers.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace CSharpWars.Validator.Helpers;

public class ScriptValidationHelper : IScriptValidationHelper
{
    public async Task<ScriptValidationResponse> Validate(ScriptValidationRequest request)
    {
        var scriptValidation = new ScriptValidationResponse();

        var botScript = await PrepareScript(request.Script);

        ImmutableArray<Diagnostic> diagnostics;

        using (var sw = new SimpleStopwatch())
        {
            diagnostics = botScript.Compile();
            scriptValidation.CompilationTimeInMilliseconds = sw.ElapsedMilliseconds;
        }

        if (!diagnostics.Any(x => x.Severity == DiagnosticSeverity.Error))
        {
            var task = Task.Run(() =>
            {
                var arena = new ArenaDto(10, 10);
                var bot = new BotDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Bot",
                    MaximumHealth = 100,
                    CurrentHealth = 100,
                    MaximumStamina = 100,
                    CurrentStamina = 100,
                    X = 1,
                    Y = 1,
                    Orientation = PossibleOrientations.South,
                    Memory = new Dictionary<string, string>().Serialize()
                };
                var friendBot = new BotDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Friend",
                    MaximumHealth = 100,
                    CurrentHealth = 100,
                    MaximumStamina = 100,
                    CurrentStamina = 100,
                    X = 1,
                    Y = 3,
                    Orientation = PossibleOrientations.North
                };
                var enemyBot = new BotDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Enemy",
                    MaximumHealth = 100,
                    CurrentHealth = 100,
                    MaximumStamina = 100,
                    CurrentStamina = 100,
                    X = 1,
                    Y = 5,
                    Orientation = PossibleOrientations.North
                };
                var botProperties = BotProperties.Build(bot, arena, new[] { bot, friendBot, enemyBot }.ToList());
                var scriptGlobals = ScriptGlobals.Build(botProperties);
                using (var sw = new SimpleStopwatch())
                {
                    try
                    {
                        botScript.RunAsync(scriptGlobals).Wait();
                    }
                    catch (Exception ex)
                    {
                        scriptValidation.ValidationMessages.Add(new ScriptValidationMessage
                        {
                            Message = "Runtime error: " + ex.Message
                        });
                    }
                    scriptValidation.RunTimeInMilliseconds = sw.ElapsedMilliseconds;
                }
            });

            if (!task.Wait(TimeSpan.FromSeconds(2)))
            {
                scriptValidation.ValidationMessages.Add(new ScriptValidationMessage
                {
                    Message = "Your script did not finish in a timely fashion!"
                });

                scriptValidation.RunTimeInMilliseconds = long.MaxValue;
            }
        }

        foreach (var diagnostic in diagnostics)
        {
            if (diagnostic.Severity == DiagnosticSeverity.Error)
            {
                scriptValidation.ValidationMessages.Add(new ScriptValidationMessage
                {
                    Message = diagnostic.GetMessage(),
                    LocationStart = diagnostic.Location.SourceSpan.Start,
                    LocationEnd = diagnostic.Location.SourceSpan.End
                });
            }
        }

        return scriptValidation;
    }

    private Task<Script<object>> PrepareScript(string script)
    {
        return Task.Run(() =>
        {
            var decodedScript = script.Base64Decode();
            var mscorlib = typeof(object).Assembly;
            var systemCore = typeof(Enumerable).Assembly;
            var dynamic = typeof(DynamicAttribute).Assembly;
            var csharpScript = typeof(BotProperties).Assembly;
            var enums = typeof(PossibleMoves).Assembly;
            var scriptOptions = ScriptOptions.Default.AddReferences(mscorlib, systemCore, dynamic, csharpScript, enums);
            scriptOptions = scriptOptions.WithImports("System", "System.Linq", "System.Collections", "System.Collections.Generic", "CSharpWars.Enums", "CSharpWars.Scripting", "CSharpWars.Scripting.Model", "System.Runtime.CompilerServices");
            var botScript = CSharpScript.Create(decodedScript, scriptOptions, typeof(ScriptGlobals));
            botScript.WithOptions(botScript.Options.AddReferences(mscorlib, systemCore));
            botScript.Compile();
            return botScript;
        });
    }
}