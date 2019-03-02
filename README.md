# CSharpWars

![CSharp Wars Logo](https://www.djohnnie.be/csharpwars/logo.png "CSharp Wars Logo")

| Build | Status |
|-------|--------|
| CI | [![Build Status](https://involvedcloud.visualstudio.com/CSharp-Wars/_apis/build/status/Djohnnie.CSharpWars?branchName=master)](https://involvedcloud.visualstudio.com/CSharp-Wars/_build/latest?definitionId=54&branchName=master) |


* [Part 1 - Forming the Idea](#part-1---forming-the-idea)
* [Part 2 - Implementing a simple API](#part-2---implementing-a-simple-api)
* [Part 3 - Implementing a Unity3D Client](#part-3---implementing-a-unity3d-client)
* [Part 4 - Implementing a Scripting Middleware](#part-4---implementing-a-scripting-middleware)
* [Part 5 - Putting everything together](#part-5---putting-everything-together)
* [Appendix I - Project Structure](#appendix-i---project-structure)
* [Appendix II - Setup and Run](#appendix-ii---setup-and-run)


## Part 1 - Forming the Idea

### Context

I have been working as a software development consultant and C# and .NET teacher for the past years. Sometimes developers and students can use some extra fun to awaken their creativity and improve their enthusiasm. In order to make students have some fun while learning and make developers creative while being competitive, I wanted to create a game that can be scripted using C#.

### Game

The easiest idea for a game that can be scripted using .NET and C# is a death match game where players fight a match inside an arena. Each player can script one, or even multiple robots inside the arena. Once a robot has been scripted and deployed into the arena, its future is entirely dependent of the script. It cannot be changed, and it cannot be revoked.

##### Scripting

The game will run based on turns and all robots will execute their turn simultaneously. A single turn is based on a script that the player will write using C#. Because a robot can be deployed using one script, each turn is based on the same script. The script should be advanced enough to execute the correct move for the correct conditions, but only one move (the first) for each turn will be accepted.
To improve predictability, all attack related turns are executed first and the turns related to movement are executed last.
The scripting context will provide the player with the needed information about his own robot, but also about the robots that are visible to his own robot. This way the robot can make decisions based on this data.

##### Moves

In order to give the player a variety of options, he can use a number of different moves in his scripts to make his robot walk around the arena and fight other robots. Because a robot is governed by a health and stamina property, his time in the arena is limited and he must use his stamina wisely and take care of his health.

A robot has five options to move around:

| Move | Description |
|------|-------------|
| **WalkForward** | Performing this move makes the robot walk forward in the direction he is currently oriented. This move consumes one stamina point. |
| **TurnLeft** | Performing this move makes the robot turn anti-clockwise by 90°. This move does not consume stamina because the robot will not move away from its current location in the arena grid. |
| **TurnRight** | Performing this move makes the robot turn clockwise by 90°. This move does not consume stamina because the robot will not move away from its current location in the arena grid. |
| **TurnAround** | Performing this move makes the robot turn 180°. This move does not consume stamina because the robot will not move away from its current location in the arena grid. |
| **Teleport** | Performing this move makes the robot jump to a new location within a predefined range, keeping the same orientation. This move consumes 20 points of stamina. |

A robot has three options to fight:

| Move | Description |
|------|-------------|
| **MeleeAttack** | Performing this attack makes the robot punch whatever is in front of him. The amount of damage caused is dependent on the orientation of the victim. Performing a backstab melee attack does more damage then punchin a robot in the face. |
| **RangedAttack** | Performing this attack makes the robot throw an object to a specific location, causing minimal damage. The ranged attack is limited in range and trying to throw beyond this limit can cause unexpected results. |
| **SelfDestruct** | Performing this attack kills your robot instantly, causing a huge amount of damage to the robots surrounding you. This attack is only used in extreme hopeless situations. |

A robot has three additional things that can happen to him:

| Move | Description |
|------|-------------|
| **Idling** | This action describes that the robot did not perform a move and thus stays in the exact same position. |
| **Died** | This action describes that the robot died because his health reached zero or lower. |
| **ScriptError** | This action describes that the robot crashed because of a script error. |

### Technology

Because I am a developer myself and I obviously wanted to have some fun, I took this opportunity to dive into some new technologies.

##### Frontend
The game itself should have an attractive frontend to display the battle of robots inside the arena. The **[Unity Game Engine](https://unity3d.com/)** promises to provide a quick and easy platform to create stunning 3D environments.

A web-based UI can be used for players to write C# scripts and deploy them to the game arena. Because this part of the game is not the main focus of this project, I will keep it simple and develop it using **[ASP.NET Core MVC](https://github.com/dotnet/core)**.

##### Middleware
Running C# scripts dynamically should be doable using the **[Microsoft Compiler Platform (Roslyn)](https://github.com/dotnet/roslyn)**.

##### Backend
Communication between frontends and backend can be easily provided using an HTTP based technology. I will use **[ASP.NET Core WebApi](https://github.com/dotnet/core)** to implement this.

Data will be stored in a single relation database using **[Microsoft SQL Server for Linux on Docker](https://hub.docker.com/_/microsoft-mssql-server)**.

##### Deployment
Because I want flexibility in deployment, **[Docker Containers](https://www.docker.com/)** will be used to host the backend web applications and middleware service. For building code and Docker containers, **[Azure DevOps & Azure Pipelines](https://azure.microsoft.com/nl-nl/services/devops/pipelines/)** will be used.

## Part 2 - Implementing a simple API

Coming soon...

## Part 3 - Implementing a Unity3D Client

Coming soon...

## Part 4 - Implementing a Scripting Middleware

Coming soon...

## Part 5 - Putting everything together

Coming soon...

## Appendix I - Project Structure

Coming soon...

## Appendix II - Setup and Run

Coming soon...
