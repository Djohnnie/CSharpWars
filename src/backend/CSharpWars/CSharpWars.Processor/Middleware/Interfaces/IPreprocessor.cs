﻿namespace CSharpWars.Processor.Middleware.Interfaces;

public interface IPreprocessor
{
    Task Go(ProcessingContext context);
}