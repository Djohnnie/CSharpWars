﻿namespace CSharpWars.Processor.Middleware.Interfaces;

public interface IPostprocessor
{
    Task Go(ProcessingContext context);
}