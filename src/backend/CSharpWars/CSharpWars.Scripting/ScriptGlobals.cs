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
        public Orientation Orientation => _50437079C366407D978Fe4Afd60C535F.Orientation;
        public Move LastMove => _50437079C366407D978Fe4Afd60C535F.LastMove;
        public Int32 MaximumPhysicalHealth => _50437079C366407D978Fe4Afd60C535F.MaximumPhysicalHealth;
        public Int32 CurrentPhysicalHealth => _50437079C366407D978Fe4Afd60C535F.CurrentPhysicalHealth;
        public Int32 MaximumStamina => _50437079C366407D978Fe4Afd60C535F.MaximumStamina;
        public Int32 CurrentStamina => _50437079C366407D978Fe4Afd60C535F.CurrentStamina;

        #endregion

        #region <| Constants |>

        // Damage inflicted on melee attack.
        public const Int16 MELEE_DAMAGE = 10;
        // Damage inflicted on melee attack from behind.
        public const Int16 MELEE_BACKSTAB_DAMAGE = 15;
        // Damage inflicted on ranged attack.
        public const Int16 RANGED_DAMAGE = 1;
        // Damage inflicted on neighboring bots, range 1, on self destruction.
        public const Int16 SELF_DESTRUCT_MAX_DAMAGE = 50;
        // Damage inflicted on neighboring bots, range 2, on self destruction.
        public const Int16 SELF_DESTRUCT_MED_DAMAGE = 10;
        // Damage inflicted on neighboring bots, range 3, on self destruction.
        public const Int16 SELF_DESTRUCT_MIN_DAMAGE = 2;
        // Maximum range for ranged attacks.
        public const Int16 MAXIMUM_RANGE = 6;
        // Maximum range for teleporting.
        public const Int16 MAXIMUM_TELEPORT = 6;

        public const Move IDLING = Move.Idling;
        public const Move TURNING_LEFT = Move.TurningLeft;
        public const Move TURNING_RIGHT = Move.TurningRight;
        public const Move TURNING_AROUND = Move.TurningAround;
        public const Move MOVING_FORWARD = Move.MovingForward;
        public const Move RANGED_ATTACK = Move.RangedAttack;
        public const Move MELEE_ATTACK = Move.MeleeAttack;
        public const Move SELF_DESTRUCTING = Move.SelfDestruct;
        public const Move SCRIPT_ERROR = Move.ScriptError;
        public const Move DYING = Move.Died;
        public const Move TELEPORTING = Move.Teleport;

        public const Orientation NORTH = Orientation.North;
        public const Orientation EAST = Orientation.East;
        public const Orientation SOUTH = Orientation.South;
        public const Orientation WEST = Orientation.West;

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
            SetCurrentMove(Move.MovingForward);
        }

        /// <summary>
        /// Calling this method will turn the player 90 degrees to the left.
        /// </summary>
        public void TurnLeft()
        {
            SetCurrentMove(Move.TurningLeft);
        }

        /// <summary>
        /// Calling this method will turn the player 90 degrees to the right.
        /// </summary>
        public void TurnRight()
        {
            SetCurrentMove(Move.TurningRight);
        }

        /// <summary>
        /// Calling this method will turn the player 180 degrees around.
        /// </summary>
        public void TurnAround()
        {
            SetCurrentMove(Move.TurningAround);
        }

        /// <summary>
        /// Calling this method will self destruct the player resulting in its death.
        /// </summary>
        public void SelfDestruct()
        {
            SetCurrentMove(Move.SelfDestruct);
        }

        /// <summary>
        /// Calling this method will execute a melee attack.
        /// </summary>
        public void MeleeAttack()
        {
            SetCurrentMove(Move.MeleeAttack);
        }

        /// <summary>
        /// Calling this method will execute a ranged attack to the specified location.
        /// </summary>
        public void RangedAttack(Int32 x, Int32 y)
        {
            SetCurrentMove(Move.RangedAttack);
        }

        /// <summary>
        /// Calling this method will teleport the player to the specified location.
        /// </summary>
        public void Teleport(Int32 x, Int32 y)
        {
            SetCurrentMove(Move.Teleport);
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

        private void SetCurrentMove(Move currentMove)
        {
            if (_50437079C366407D978Fe4Afd60C535F.CurrentMove == Move.Idling)
            {
                _50437079C366407D978Fe4Afd60C535F.CurrentMove = currentMove;
            }
        }

        #endregion
    }
}