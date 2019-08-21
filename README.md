# CSharpWars

![CSharp Wars Logo](https://www.djohnnie.be/csharpwars/logo.png "CSharp Wars Logo")

| Build | Status | Tests | Coverage | Deployment |
|-------|--------|-------|----------|------------|
| CSharpWars CI | [![Build Status](https://dev.azure.com/djohnnieke/CSharpWars/_apis/build/status/Djohnnie.CSharpWars?branchName=master)](https://dev.azure.com/djohnnieke/CSharpWars/_build/latest?definitionId=4&branchName=master) | ![Azure DevOps tests](https://img.shields.io/azure-devops/tests/djohnnieke/csharpwars/4/master) | ![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/djohnnieke/csharpwars/4/master) | ![Deployment Status](https://vsrm.dev.azure.com/djohnnieke/_apis/public/Release/badge/bcd38fdd-dc13-4325-bee3-9112645bbde6/1/1) |

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

CSharpWars is not a hardcore game that needs real-time server/client communication, so I opted for a very simple approach of using HTTP APIs for communication between the game front-end and the back-end. The state of the game world will be stored inside a relational database, with entities like Player and Bot, and will only be updated by the processing middleware once every two seconds.  The processing middleware takes the scripts from the database, compiles, initializes and later runs them for all robots in parallel. If the game front-end polls the game state once every two seconds, animating the assets between their previous and current state should be sufficient.

### Entities

The relational database will contain a list of robots, storing their state, and a list of players, grouping their deployed robots. When deploying a robot, a player needs to provide a C# script to define the behavior of the robot. This C# script is only needed once by the processing middleware and is therefore accessed using a separate entity. It is however stored in the same table as the robot state itself.

![CSharpWars Entities](https://www.djohnnie.be/csharpwars/entities.png "CSharpWars Entities")

### HTTP API

Because I want to use this project to play around with .NET Core 3, ASP.NET Core WebAPI is my choice as the technology for the HTTP APIs. The only important component that will use the HTTP API for now, is the game front-end. Because of this, only an endpoint on the robot entity is required. This endpoint will return all active robots, which are robots that have not died, or that have died within the last 10 seconds. The game front-end needs these 10 seconds to animate the death of the robot because face it: we like to see virtual robots die! In the future, multiple arenas will be supported. Right now, the arena endpoint will always return a single arena instance with a predefined width and height.

![API](https://www.djohnnie.be/csharpwars/api.png "API")

The ASP.NET Core 3 application is dockerized and runs on a Linux based Synology NAS for demo purposes.

Fetching and storing data in the relational database is performed by Entity Framework Core 3. A table for players and a table for robots are mapped to Player, Bot and BotScript entities. Nothing fancy there.

Just for the fun of it, Microsoft SQL Server from a Docker Container also runs on the same Synology NAS.

### Scripting Middleware

The scripting middleware is a .NET Core 3 Console application using The Microsoft Compiler Platform, also known as Roslyn, to compile and run robot scripts. If the Console application is running, it will trigger a processor once every two seconds to run all active robot scripts in parallel. Running a robot script will take place in three stages:

![Middleware Processing](https://www.djohnnie.be/csharpwars/middleware-processing.png "Middleware Processing")

```c#
public async Task Process()
{
    var arena = await _arenaLogic.GetArena();
    var bots = await _botLogic.GetAllLiveBots();
    var context = ProcessingContext.Build(arena, bots);

    await _preprocessor.Go(context);
    await _processor.Go(context);
    await _postprocessor.Go(context);

    await _botLogic.UpdateBots(bots);
}
```

1. **Preprocessing**: This stage will prepare an object model containing all active robots, their current stats, memory and their awareness of other robots. Their current stats are a representation of their current state. Their memory is managed by the script that is able to use a number of functions to persist data between game turns. Awareness of other robots is determined based on the orientation of the current robot and the location of the other robots.
2. **Processing**: This stage will compile, initialize and run the actual bot scripts for all active bots in parallel. Thanks to the Microsoft Compiler Platform, a simple String object, containing C# code can be compiled and run from memory using the C# Scripting API. From these scripts, the processing stage will distill a list of moves that need to be performed by all robots.
3. **Postprocessing**: This stage will perform the listed moves one by one if possible and update the object model to reflect the new game state.

The core idea is that every robot script can result in an actual move that needs to be performed by that robot. If a script tries to perform multiple moves within the same turn, only the first move is registered and any additional moves are ignored. The moves are then categorized and prioritized in order to create a trustworthy result when all robots are performing their moves simultaneously. For example, attacks are executed before moves in order to make sure that a robot cannot escape an attack within the same turn.

Thanks to the Microsoft Compiler Platform Scripting API, a C# script without a class or method context, can be written and run from memory. As a help, I can make the script run inside the context of a class that is part of my Scripting Middleware and thus add some context to that script. In the Scripting Middleware, this context is called ScriptGlobals.

```c#
public async Task Process(BotDto bot, ProcessingContext context)
{
    var botProperties = context.GetBotProperties(bot.Id);
    try
    {
        var botScript = await GetCompiledBotScript(bot);
        var scriptGlobals = ScriptGlobals.Build(botProperties);
        await botScript.RunAsync(scriptGlobals);
    }
    catch
    {
        botProperties.CurrentMove = PossibleMoves.ScriptError;
    }
}
```

The ScriptGlobals class contains a number of Properties and Methods that are available to the player to call from within their script. Using this context, a player can write an intelligent script that makes a decision based on these Properties and calls a Method to perform a move.

```c#
public Int32 Width { get; }
public Int32 Height { get; }
public Int32 X { get; }
public Int32 Y { get; }
public PossibleOrientations Orientation { get; }
public PossibleMoves LastMove { get; }
public Int32 MaximumHealth { get; }
public Int32 CurrentHealth { get; }
public Int32 MaximumStamina { get; }
public Int32 CurrentStamina { get; }
public Vision Vision { get; }

public void WalkForward();
public void TurnLeft();
public void TurnRight();
public void TurnAround();
public void SelfDestruct();
public void MeleeAttack();
public void RangedAttack(Int32 x, Int32 y);
public void Teleport(Int32 x, Int32 y);

public void StoreInMemory<T>(String key, T value);
public T LoadFromMemory<T>(String key);
public void RemoveFromMemory(String key);
public void Talk(String message);
```

## Part 3 - Implementing a Unity3D Client

### Context

My decision to create a 3D environment to visualize the arena and fighting robots made me look into the Unity Game Engine. As a professional .NET backend developer I should never need a game engine, so this is an opportunity to learn something new and challenging. Today, Unity is a very popular tool to create both small and large games. Unity uses the Mono project to offer a choice, next to JavaScript, to use C# as a programming language for its scripting. Because I am a C# developer since the start of .NET, this made my leap into Unity a lot more familiar.

![Unity Project](https://www.djohnnie.be/csharpwars/unity-project.png "Unity Project")

### Platform

Unity allows me to compile my project to a multitude of platforms like Desktop, Mobile, Web, and Console. For this project, I decided to compile to native Windows for Desktop and optionally to the web using WebGL. The desktop version will be a Windows executable with some supporting files that can be shared in a ZIP archive or an installer. The WebGL version can be integrated into a webpage to make it available through the web browser.

### Assets

Unity provides a user interface to manage a number of assets. Games generally use a large number of assets that make up the entire visual world but also support the dynamics of this world through scripts. CSharpWars uses a number of different assets like scenes, models, animations, textures, materials, prefabs, scripts and many others.

Assets can be created with a range of supporting tools, like 3d modeling software. Since I am not a graphical wizard, I downloaded a number of free assets from different locations in order to prototype a working game front-end. Unity provides a built-in Unity Asset Store to download both free and paid assets and use them directly in your games. Some other assets, like textures, can be downloaded from a wide range of websites.

**A scene** contains your environment and UI. Think of a scene as a unique level. A scene will generally contain a camera to provide a viewport for the player looking at the scene and any number of lights to create a realistic view of the game world. CSharpWars only needs a single environment, containing a platform or arena hosting a number of fighting robots. It also doesn’t contain any UI or menu system and will immediately show the arena and active fight when started. It will contain a single light floating in the air as if it were the sun and it will contain a camera, slowly rotating around the arena, to create a dynamic view of the battlefield.

**Models** are graphical 3d representations of objects. They exist of polygon meshes and can be placed inside of your 3d environment. CSharpWars uses a simple cube model and transforms it into an arena floor by extending its width, depth, and height. Width and depth are based on the arena dimensions. Height is a small value to make the arena into a thin, but visible, floating floor surface. Other models for robots and effects are downloaded from the Unity Asset Store for free.

**Animations** are bound to models and are able to animate the position and rotation of individual meshes to create moving models. CSharpWars only uses animations to animate robots from their previous game state to their current game state. The animations for this were included within the robot assets package downloaded from the Unity Asset Store.

**Textures** are images that makeup mesh surfaces. They are responsible for making a model look realistic. A texture is a simple image that can be wrapped around a mesh surface. All textures in CSharpWars were included with the assets downloaded from the Unity Asset Store or were downloaded from other websites containing free surface texture images.

**Materials** are assets that take a texture and are linked to a mesh surface. It will use parameters to decide the look and feel of the texture based on known materials like wood, metal or whatever you’d like. An important feature of a material is how it will react to light. Light can be reflected, refracted or absorbed, creating a distinct look and feel.

**Prefabs** are used as blueprints of physical objects that are used inside your environment. They can contain a collection of assets that together make up a more complex object. This object can easily be instantiated and cloned to make object management a lot easier to handle. CSharpWars uses prefabs for its robots because each robot is built using the same structure. Whenever a new robot is deployed to the arena, an instance of this prefab is created and placed on the correct location in the arena. The instance will contain all properties, parameters, and scripts that are needed for the object to live independently of the others.

**Scripts** are pieces of C# code that can be linked to different kinds of assets or game objects built from assets. From the script, you are able to change the properties of the attached game object to make your game dynamic and react to certain triggers. CSharpWars does not use a single script with a game-loop but uses a number of prefabs and game objects with linked scripts.

### Controllers

I have called a number of my C# scripts, controllers. These scripts are linked to a game object that represents an important aspect of the environment and evaluates that game object every frame tick of the running game.

The arena itself, a surface defined by a width and height, is managed by an ArenaController. This controller uses another helper script to call the HTTP API endpoint to get the details for the arena. Based on the result, the ArenaController will transform the arena floor to match these dimensions.

On top of the arena, a BotsController is managing the collection of fighting robots. Again by using a helper script, a call to the HTTP API endpoint is executed every two seconds to get the latest list of active robots. If a managed robot is gone from the list, the robot is removed from the arena and all references are cleaned. If a new robot is discovered, the script will instantiate a new robot instance from the prefab and will start managing it. If an existing robot is still in the list, its state is updated.

Next to the BotsController, each robot instance is managed by a BotController. This controller will be notified if the state for the robot has changed and will trigger actions or animations based on this. The BotController will perform frame-by-frame evaluations to correctly animate a robot performing a move like walking, turning, fighting or even dying.

A number of smaller controllers will manage things like flying particles due to ranged attacks, explosions due to self destructs, tags for displaying robot names, health and stamina bars.

## Appendix I - Project Structure

Coming soon...

## Appendix II - Setup and Run

Coming soon...
