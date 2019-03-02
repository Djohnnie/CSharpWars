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

### Game?

The easiest idea for a game that can be scripted using .NET and C# is a deathmatch game where players fight a match inside an arena. Each player can script one, or even multiple robots inside the arena. Once a robot has been scripted and deployed into the arena, its future is entirely dependent of the script. It cannot be changed, and it cannot be revoked.

#### Scripting

#### Moves

In order to give the player a variety of options, he can use a number of different moves in his scripts to make his robot walk around the arena and fight other robots. Because a robot is governed by a health and stamina property, his time in the arena is limited and he must use his stamina wisely and take care of his health.

A robot has five options to move around:

| Move | Description |
|------|-------------|
| **WalkForward** | Performing this move makes the robot walk forward in the direction he is currently oriented. This move consumes one stamina point. |
| **TurnLeft** | Performing this move makes the robot turn anti-clockwise by 90°. This move does not consume stamina because the robot will not move away from it's current location in the arena grid. |
| **TurnRight** | Performing this move makes the robot turn clockwise by 90°. This move does not consume stamina because the robot will not move away from it's current location in the arena grid. |
| **TurnAround** | Performing this move makes the robot turn 180°. This move does not consume stamina because the robot will not move away from it's current location in the arena grid. |
| **Teleport** | Performing this move makes the robot jump to a new location within a predefined range, keeping the same orientation. This move consumes 20 points of stamina. |

A robot has three options to fight:

| Move | Description |
|------|-------------|
| **MeleeAttack** | Performing this attack makes the robot punch whatever is in front of him. The amount of damage caused is dependend on the orientation of the victim. Performing a backstab melee attack does more damage then punchin a robot in the face. |
| **RangedAttack** | Performing this attack makes the robot throw an object to a specific location, causing minimal damage. The ranged attack is limited in range and trying to throw beyond this limit can cause unexpected results. |
| **SelfDestruct** | Performing this attack kills your robot instantly, causing a huge amount of damage to the robots surrounding you. This attack is only used in extreme hopeless situations. |

A robot has three additional things that can happen to him:

| Move | Description |
|------|-------------|
| **Idling** | This action describes that the robot did not perform a move and thus stays in the exact same position. |
| **Died** | This action describes that the robot died because his health reached zero or lower. |
| **ScriptError** | This action describes that the robot crached because of a script error. |

### Technology?

Because I am a developer myself and I obviously wanted to have some fun, I took this opportunity to dive into some new technologies. At the core of the game itself, because it should be possible to script robots using C#, I looked at the **Microsoft Compiler Platform (Roslyn)**. As a total newbie I wanted to challenge myself and use the **Unity Game Engine** to create a good looking 3D client application to display the arena with fighting robots. For communication between frontent, middleware and backend I chose to use **ASP.NET Core WebApi** and **Microsoft SQL Server** for storing simple relational data.

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
