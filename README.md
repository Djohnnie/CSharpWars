# CSharpWars

![CSharp Wars Logo](https://www.djohnnie.be/csharpwars/logo.png "CSharp Wars Logo")

| Build | Status |
|-------|--------|
| CSharpWars CI | [![Build Status](https://involvedcloud.visualstudio.com/CSharp-Wars/_apis/build/status/Djohnnie.CSharpWars?branchName=master)](https://involvedcloud.visualstudio.com/CSharp-Wars/_build/latest?definitionId=54&branchName=master) |

| Docker Hub | | |
|------------|--------------|----------------|
| [djohnnie/csharpwars-api](https://hub.docker.com/r/djohnnie/csharpwars-api) | [![](https://images.microbadger.com/badges/image/djohnnie/csharpwars-api.svg)](https://microbadger.com/images/djohnnie/csharpwars-api "Get your own image badge on microbadger.com") | [![](https://images.microbadger.com/badges/version/djohnnie/csharpwars-api.svg)](https://microbadger.com/images/djohnnie/csharpwars-api "Get your own version badge on microbadger.com") |
| [djohnnie/csharpwars-processor](https://hub.docker.com/r/djohnnie/csharpwars-processor) | [![](https://images.microbadger.com/badges/image/djohnnie/csharpwars-processor.svg)](https://microbadger.com/images/djohnnie/csharpwars-processor "Get your own image badge on microbadger.com") | [![](https://images.microbadger.com/badges/version/djohnnie/csharpwars-processor.svg)](https://microbadger.com/images/djohnnie/csharpwars-processor "Get your own version badge on microbadger.com") |

## Table of Contents

* [Part 1 - Forming the Idea](#part-1---forming-the-idea)
* [Part 2 - Implementing a simple HTTP API and the Scripting Middleware](#part-2---implementing-a-simple-http-api-and-the-scripting-middleware)
* [Part 3 - Implementing a Unity3D Client](#part-3---implementing-a-unity3d-client)
* [Appendix I - Project Structure](#appendix-i---project-structure)
* [Appendix II - Setup and Run](#appendix-ii---setup-and-run)


## Part 1 - Forming the Idea

![CSharpWars Banner](https://www.djohnnie.be/csharpwars/banner.jpg "CSharpWars Banner")

### Context

I have been working as a software development consultant and C# and .NET teacher for over 10 years. Developers and students can use some extra fun to awaken their creativity and improve their enthusiasm and spark. In order to make students have some fun while learning and make developers creative while being competitive, I created a simple game that can be scripted using C#. Creating this game was a fun challenge for me because it provided me with the opportunity to learn and experiment with some new technologies. I keep this idea for a scripting game around and alive as a playground to use in schools and on company teambuilding events.

### Game

The most interesting idea that I could think of for a game that can be scripted using .NET and C# is a deathmatch game where players fight a match inside a square and empty arena. Each player can script one or more robots inside the arena using his knowledge of basic programming logic. Once a robot has been fully scripted and deployed into the arena, its future is entirely dependent on the script. The script cannot be changed and it cannot be revoked. The scripting itself is used to move the robot around the arena and to fight robots from other players by making the correct decisions based on data fed to the robot script. A robot wins if it can stay alive in a "last man standing" kind of tournament. However, the purpose in this game is not to win, but to write an intelligent script, which is harder than you would think.

##### Scripting

```c#
var step = LoadFromMemory<Int32>( "STEP" );
if( step % 3 == 0 ) { TurnLeft(); } else { WalkForward(); }
step++;
StoreInMemory( "STEP" , step );
```

The game-loop will run based on turns and all robots will execute their turn simultaneously. A single turn is based on the C# script that the player has written. Because a robot can be deployed using one script, each turn is based on the same script. The script should be intelligent enough to execute the correct move for the correct conditions, but only one move (the first) for each turn will be accepted and eventually executed by the game-loop.
To improve predictability, all attack related turns are executed first and the turns related to movement are executed last.
The scripting context will provide the player with the needed information about his own robot, but also about the robots that are visible to his robot.

##### Moves

A robot can walk around the arena and fight other robots in order to win by combining basic C#, .NET logic and a list of predefined moves. Because a robot is governed by a health and stamina property, his time in the arena is limited and he must use his stamina wisely and take care of his health.

In a single turn, your robot can walk forward, turn 90 degrees in each direction or completely turn around 180 degrees. You can even teleport your robot to another location within a predefined range to be able to cover more distance. Actually moving to another location will drain stamina from your available pool, turning on the spot will be free. Additionally you can fight using a melee kick or hit, or you can throw objects within a predefined range to cause damage and drain health from your opponents. If you are a kamikaze, you can even make your robot self destruct and cause mayhem to all robots in your vicinity. Because the same script is run on each turn, it could be helpful to store and persist data between turns. Each robot will also be able to see the space in front of him by looking to the arena. This will give you the data needed to pursue the demise of your worst enemy within the arena!

### Technology

Because I am not a game developer, but I obviously wanted to have some fun, I used some technologies that are completely new to me. For other components and technologies, I used familiar stuff, but just went bleeding edge and used the latest preview versions available.

##### Frontend
The game itself should have an attractive frontend to display the death match battle of robots inside the arena. The **[Unity Game Engine](https://unity3d.com/)** promises to provide a quick and easy platform to create stunning 3D environments, combined with C# to write its game logic. The Unity Game Engine can compile to native Windows applications, but also supports a wide range of other platforms including WebGL.

A web-based UI can be used for players to write C# scripts and deploy them to the game arena. Because this part of the game is currently not the main focus of this project, I will keep it simple and develop it using **[ASP.NET Core MVC](https://github.com/dotnet/core)**.

##### Middleware
Running C# scripts dynamically should be doable using the **[Microsoft Compiler Platform (Roslyn)](https://github.com/dotnet/roslyn)**. This framework provides access to the C# Compiler by feeding it C# code as-a-string. If I feed the compiled C# script some context, the script itself can alter this context which I can then feed to the game-loop. For simplicity and flexibility, I will implement this middleware game-loop logic as a Console Application.

##### Backend
I will provide communication between frontends and backend using a simple HTTP based technology like **[ASP.NET Core WebApi](https://github.com/dotnet/core)**. The frontend game client should be able to poll the game state and the frontend web client should be able to deploy new robots and scripts to the game-loop.

Data will be stored in a single relation database using **[Microsoft SQL Server for Linux on Docker](https://hub.docker.com/_/microsoft-mssql-server)** or **[Azure SQL](https://azure.microsoft.com/nl-nl/free/sql-database)**.

##### Deployment
Because I want flexibility in deployment, I use **[Docker Containers](https://www.docker.com/)** to host the backend web applications and middleware service. I use **[Azure DevOps & Azure Pipelines](https://azure.microsoft.com/nl-nl/services/devops/pipelines/)** to automatically compile code, run unit tests and build Docker containers.

### Finally

In the next chapter, I will discuss development of the backend HTTP API and scripting middleware in more detail.

## Part 2 - Implementing a simple HTTP API and the Scripting Middleware

Coming soon...

## Part 3 - Implementing a Unity3D Client

Coming soon...

## Appendix I - Project Structure

Coming soon...

## Appendix II - Setup and Run

Coming soon...
