# CSharpWars

![CSharp Wars Logo](https://www.djohnnie.be/csharpwars/logo.png "CSharp Wars Logo")

| Build | Status | Tests | Coverage |
|-------|--------|-------|---------------|
| CSharpWars CI | [![Build Status](https://involvedcloud.visualstudio.com/CSharp-Wars/_apis/build/status/Djohnnie.CSharpWars?branchName=master)](https://involvedcloud.visualstudio.com/CSharp-Wars/_build/latest?definitionId=54&branchName=master) | ![Azure DevOps tests](http://my.djohnnie.be:8088/azure-devops/tests/involvedcloud/csharp-wars/54.svg?logo=visual-studio-code&label=UnitTests) | ![Azure DevOps coverage](http://my.djohnnie.be:8088/azure-devops/coverage/involvedcloud/csharp-wars/54.svg?logo=visual-studio-code&label=Code%20Coverage) | 

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

The game-loop will run based on turns and all robots will execute their turn simultaneously. A single turn is based on the C# script that the player has written. Because a robot can be deployed using one script only, each turn is based on the same script. The script should be intelligent enough to execute the correct move for the correct conditions, but only one move (the first) for each turn will be accepted and eventually executed by the game-loop.
To improve predictability, all attack related turns are executed first and the turns related to movement are executed last.
The scripting context will provide the player with the needed information about his own robot, but also about the robots that are visible to his robot.

##### Moves

A robot can walk around the arena and fight other robots in order to win by combining basic C#, .NET logic and a list of predefined moves. Because a robot is governed by a health and stamina property, his time in the arena is limited and he must use his stamina wisely and take care of his health.

In a single turn, the players' robot can walk forward, turn 90 degrees in each direction or completely turn around 180 degrees. The player can even teleport its robot to another location within a predefined range to be able to cover more distance. Actually moving to another location will drain stamina from its available pool, but turning on the spot will be free. Additionally the player can make its robot fight using a melee kick or hit, or throw objects within a predefined range to cause damage and drain health from its opponents. If the players' robot is a kamikaze, he can even make it selfdestruct and cause mayhem to all robots in its vicinity. Because the same script is run on each turn, it could be helpful to store and persist data between turns. Each robot will also be able to see the space in front of him by looking to the arena. This will provide the robot the data needed to pursue the demise of its worst enemy within the arena!

### Technology

Because I am not a game developer, but I obviously wanted to have some fun, I used some technologies that are completely new to me. For other components and technologies, I used familiar stuff, but just went bleeding edge and used the latest preview versions available.

##### Frontend
The game itself should have an attractive frontend to display the deathmatch battle of robots inside the arena. The **[Unity Game Engine](https://unity3d.com/)** promises to provide a quick and easy platform to create stunning 3D environments, combined with C# to write its game logic. The Unity Game Engine can compile to native Windows applications, but also supports a wide range of other platforms including WebGL.

The arena has a space theme and shows a two-dimensional grid floating among the stars. The camera will slowly rotate around the arena to provide a 360 degree view on the ongoing battle. Robots are represented by animated models of robots that move on this floating grid and perform their turns simultaniously with a pause of a few seconds between each turn. The game client will only present the arena and the player is not able to interact with the robots.

The player can use a web-based UI to write C# scripts and deploy them to the game arena. Because this part of the game is currently not the main focus of this project, I will keep it simple and develop it using **[ASP.NET Core MVC](https://github.com/dotnet/core)**.

##### Middleware
Running C# scripts dynamically is doable using the **[Microsoft Compiler Platform (Roslyn)](https://github.com/dotnet/roslyn)**. This framework provides access to the C# Compiler by feeding it C# code as-a-string. If I feed the compiled C# script some context, the script itself can alter this context which I can then feed into the game-loop. For simplicity and flexibility, I will implement this middleware game-loop logic as a Console Application.

Visual Studio and other tools use The **Microsoft Compiler Platform** primarily for C# compilation, Intellisense and code validation. Because the watch and immediate window in Visual Studio also use the C# compiler, the platform provides scripting possibilities on top of C#. I will use this part of the platform to compile the players' C# script and run it as part of the game-loop. If the arena contains more than one robot, the game-loop will run all compiled C# scripts in parallel and evaluate the results to update the game-state.

##### Backend
I will provide communication between frontends and backend using a simple HTTP based technology like **[ASP.NET Core WebApi](https://github.com/dotnet/core)**. The frontend game client should be able to poll the game-state and the frontend web client should be able to deploy new robots and scripts to the game-loop.

Data will be stored in a single relation database using **[Microsoft SQL Server for Linux on Docker](https://hub.docker.com/_/microsoft-mssql-server)** or **[Azure SQL](https://azure.microsoft.com/nl-nl/free/sql-database)**.

##### Deployment
Because I want flexibility in deployment, I use **[Docker Containers](https://www.docker.com/)** to host the backend web applications and middleware service. I use **[Azure DevOps & Azure Pipelines](https://azure.microsoft.com/nl-nl/services/devops/pipelines/)** to automatically compile code, run unit tests and build Docker containers.

### Finally

In the next chapter, I will discuss development of the backend HTTP API and scripting middleware in more detail.

## Part 2 - Implementing a simple HTTP API and the Scripting Middleware

### Context

CSharpWars is not a game that needs realtime server/client communication, so I opted for a very simple aproach of using HTTP API's for communication between the game front-end and the back-end. The state of the game world will be stored inside a relational database (with entities like Player and Bot) and will only be updated by the processing middleware once every two seconds. If the game front-end polls the game state once every two seconds, animating the assets between their previous and current state should be sufficient.

### Entities

![CSharpWars Entities](https://www.djohnnie.be/csharpwars/entities.png "CSharpWars Entities")

### HTTP API

Because I wanted to use this project to play around with .NET Core 3, I chose ASP.NET Core WebAPI as the technology for the HTTP API's.
The only important component that will use the HTTP API is the game front-end. Because of this, only an enpoint on the Bot entities is required.

![API](https://www.djohnnie.be/csharpwars/api.png "API")

### Scripting Middleware

The scripting middleware is a .NET Core 3 Console application using The Microsoft Compiler Platform Roslyn to compile and run bot scripts. If the Console application is running, it will trigger a processor once every two seconds to run all active bot scripts in parallel. Running the bot scripts will happen in three stages:

![Middleware Processing](https://www.djohnnie.be/csharpwars/middleware-processing.png "Middleware Processing")

1. **Preprocessing**: This stage will prepare an object model containing all active robots, their current stats and their awareness of each other.
2. **Processing**: This stage will run the actual bot scripts for all active bots in parallel and will create a list of moves that need to be performed.
3. **Postprocessing**: This stage will perform the actual moves and will mutate the active bot stats.

Every bot script can result in an actual move that needs to be performed by the robot. These moves are categorized and prioritized in order to create a trustworthy result when all robots are performing their moves simultaniously.

## Part 3 - Implementing a Unity3D Client

Coming soon...

## Appendix I - Project Structure

Coming soon...

## Appendix II - Setup and Run

Coming soon...
