using System;
using CSharpWars.Common.Extensions;
using CSharpWars.Enums;
using CSharpWars.Scripting.Model;

namespace CSharpWars.Scripting
{
    public class ScriptGlobals
    {
        #region <| Private Members |>

        private readonly BotProperties _50437079C366407D978Fe4Afd60C535F;

        #endregion

        #region <| Public Properties |>

        public Int32 Width => _50437079C366407D978Fe4Afd60C535F.Width;
        public Int32 Height => _50437079C366407D978Fe4Afd60C535F.Height;
        public Int32 X => _50437079C366407D978Fe4Afd60C535F.X;
        public Int32 Y => _50437079C366407D978Fe4Afd60C535F.Y;
        public Orientations Orientation => _50437079C366407D978Fe4Afd60C535F.Orientation;
        public Moves LastMove => _50437079C366407D978Fe4Afd60C535F.LastMove;
        public Int32 MaximumPhysicalHealth => _50437079C366407D978Fe4Afd60C535F.MaximumPhysicalHealth;
        public Int32 CurrentPhysicalHealth => _50437079C366407D978Fe4Afd60C535F.CurrentPhysicalHealth;
        public Int32 MaximumStamina => _50437079C366407D978Fe4Afd60C535F.MaximumStamina;
        public Int32 CurrentStamina => _50437079C366407D978Fe4Afd60C535F.CurrentStamina;

        #endregion

        #region <| Constants |>

        public const Int32 MELEE_DAMAGE = Constants.MELEE_DAMAGE;
        public const Int32 MELEE_BACKSTAB_DAMAGE = Constants.MELEE_BACKSTAB_DAMAGE;
        public const Int32 RANGED_DAMAGE = Constants.RANGED_DAMAGE;
        public const Int32 SELF_DESTRUCT_MAX_DAMAGE = Constants.SELF_DESTRUCT_MAX_DAMAGE;
        public const Int32 SELF_DESTRUCT_MED_DAMAGE = Constants.SELF_DESTRUCT_MED_DAMAGE;
        public const Int32 SELF_DESTRUCT_MIN_DAMAGE = Constants.SELF_DESTRUCT_MIN_DAMAGE;
        public const Int32 MAXIMUM_RANGE = Constants.MAXIMUM_RANGE;
        public const Int32 MAXIMUM_TELEPORT = Constants.MAXIMUM_TELEPORT;
        public const Int32 STAMINA_ON_MOVE = Constants.STAMINA_ON_MOVE;
        public const Int32 STAMINA_ON_TELEPORT = Constants.STAMINA_ON_TELEPORT;

        public const Moves IDLING = Moves.Idling;
        public const Moves TURNING_LEFT = Moves.TurningLeft;
        public const Moves TURNING_RIGHT = Moves.TurningRight;
        public const Moves TURNING_AROUND = Moves.TurningAround;
        public const Moves MOVING_FORWARD = Moves.WalkForward;
        public const Moves RANGED_ATTACK = Moves.RangedAttack;
        public const Moves MELEE_ATTACK = Moves.MeleeAttack;
        public const Moves SELF_DESTRUCTING = Moves.SelfDestruct;
        public const Moves SCRIPT_ERROR = Moves.ScriptError;
        public const Moves DYING = Moves.Died;
        public const Moves TELEPORTING = Moves.Teleport;

        public const Orientations NORTH = Orientations.North;
        public const Orientations EAST = Orientations.East;
        public const Orientations SOUTH = Orientations.South;
        public const Orientations WEST = Orientations.West;

        #endregion

        #region <| Construction |>

        public ScriptGlobals(BotProperties botProperties)
        {
            _50437079C366407D978Fe4Afd60C535F = botProperties;
        }

        #endregion

        #region <| Public Methods |>

        /// <summary>
        /// Calling this method will move the player one position forward.
        /// </summary>
        public void MoveForward()
        {
            SetCurrentMove(Moves.WalkForward);
        }

        /// <summary>
        /// Calling this method will turn the player 90 degrees to the left.
        /// </summary>
        public void TurnLeft()
        {
            SetCurrentMove(Moves.TurningLeft);
        }

        /// <summary>
        /// Calling this method will turn the player 90 degrees to the right.
        /// </summary>
        public void TurnRight()
        {
            SetCurrentMove(Moves.TurningRight);
        }

        /// <summary>
        /// Calling this method will turn the player 180 degrees around.
        /// </summary>
        public void TurnAround()
        {
            SetCurrentMove(Moves.TurningAround);
        }

        /// <summary>
        /// Calling this method will self destruct the player resulting in its death.
        /// </summary>
        public void SelfDestruct()
        {
            SetCurrentMove(Moves.SelfDestruct);
        }

        /// <summary>
        /// Calling this method will execute a melee attack.
        /// </summary>
        public void MeleeAttack()
        {
            SetCurrentMove(Moves.MeleeAttack);
        }

        /// <summary>
        /// Calling this method will execute a ranged attack to the specified location.
        /// </summary>
        public void RangedAttack(Int32 x, Int32 y)
        {
            SetCurrentMove(Moves.RangedAttack);
        }

        /// <summary>
        /// Calling this method will teleport the player to the specified location.
        /// </summary>
        public void Teleport(Int32 x, Int32 y)
        {
            SetCurrentMove(Moves.Teleport);
        }

        /// <summary>
        /// Calling this method will store information into the players memory.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void StoreInMemory<T>(String key, T value)
        {
            if (_50437079C366407D978Fe4Afd60C535F.Memory.ContainsKey(key))
            {
                _50437079C366407D978Fe4Afd60C535F.Memory[key] = value.Serialize();
            }
            else
            {
                _50437079C366407D978Fe4Afd60C535F.Memory.Add(key, value.Serialize());
            }
        }

        /// <summary>
        /// Calling this method will load information from the players memory.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T LoadFromMemory<T>(String key)
        {
            if (_50437079C366407D978Fe4Afd60C535F.Memory.ContainsKey(key))
            {
                return _50437079C366407D978Fe4Afd60C535F.Memory[key].Deserialize<T>();
            }
            return default(T);
        }

        /// <summary>
        /// Calling this method will remove information from the players memory.
        /// </summary>
        /// <param name="key"></param>
        public void RemoveFromMemory(String key)
        {
            if (_50437079C366407D978Fe4Afd60C535F.Memory.ContainsKey(key))
            {
                _50437079C366407D978Fe4Afd60C535F.Memory.Remove(key);
            }
        }

        /// <summary>
        /// Calling this method will make the player talk.
        /// </summary>
        /// <param name="message"></param>
        public void Talk(String message)
        {
            _50437079C366407D978Fe4Afd60C535F.Messages.Add(message);
        }

        #endregion

        #region <| Helper Methods |>

        private void SetCurrentMove(Moves currentMove)
        {
            if (_50437079C366407D978Fe4Afd60C535F.CurrentMove == Moves.Idling)
            {
                _50437079C366407D978Fe4Afd60C535F.CurrentMove = currentMove;
            }
        }

        #endregion
    }
}