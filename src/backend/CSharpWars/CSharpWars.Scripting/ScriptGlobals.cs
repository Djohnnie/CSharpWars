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
        private readonly Vision _vision;

        #endregion

        #region <| Public Properties |>

        public Int32 Width => _50437079C366407D978Fe4Afd60C535F.Width;
        public Int32 Height => _50437079C366407D978Fe4Afd60C535F.Height;
        public Int32 X => _50437079C366407D978Fe4Afd60C535F.X;
        public Int32 Y => _50437079C366407D978Fe4Afd60C535F.Y;
        public PossibleOrientations Orientation => _50437079C366407D978Fe4Afd60C535F.Orientation;
        public PossibleMoves LastMove => _50437079C366407D978Fe4Afd60C535F.LastMove;
        public Int32 MaximumHealth => _50437079C366407D978Fe4Afd60C535F.MaximumHealth;
        public Int32 CurrentHealth => _50437079C366407D978Fe4Afd60C535F.CurrentHealth;
        public Int32 MaximumStamina => _50437079C366407D978Fe4Afd60C535F.MaximumStamina;
        public Int32 CurrentStamina => _50437079C366407D978Fe4Afd60C535F.CurrentStamina;

        public Vision Vision => _vision;

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

        public const PossibleMoves IDLING = PossibleMoves.Idling;
        public const PossibleMoves TURNING_LEFT = PossibleMoves.TurningLeft;
        public const PossibleMoves TURNING_RIGHT = PossibleMoves.TurningRight;
        public const PossibleMoves TURNING_AROUND = PossibleMoves.TurningAround;
        public const PossibleMoves MOVING_FORWARD = PossibleMoves.WalkForward;
        public const PossibleMoves RANGED_ATTACK = PossibleMoves.RangedAttack;
        public const PossibleMoves MELEE_ATTACK = PossibleMoves.MeleeAttack;
        public const PossibleMoves SELF_DESTRUCTING = PossibleMoves.SelfDestruct;
        public const PossibleMoves SCRIPT_ERROR = PossibleMoves.ScriptError;
        public const PossibleMoves DYING = PossibleMoves.Died;
        public const PossibleMoves TELEPORTING = PossibleMoves.Teleport;

        public const PossibleOrientations NORTH = PossibleOrientations.North;
        public const PossibleOrientations EAST = PossibleOrientations.East;
        public const PossibleOrientations SOUTH = PossibleOrientations.South;
        public const PossibleOrientations WEST = PossibleOrientations.West;

        #endregion

        #region <| Construction |>

        private ScriptGlobals(BotProperties botProperties)
        {
            _50437079C366407D978Fe4Afd60C535F = botProperties;
            _vision = Vision.Build(botProperties);
        }

        public static ScriptGlobals Build(BotProperties botProperties)
        {
            return new ScriptGlobals(botProperties);
        }

        #endregion

        #region <| Public Methods |>

        /// <summary>
        /// Calling this method will move the player one position forward.
        /// </summary>
        public void WalkForward()
        {
            SetCurrentMove(PossibleMoves.WalkForward);
        }

        /// <summary>
        /// Calling this method will turn the player 90 degrees to the left.
        /// </summary>
        public void TurnLeft()
        {
            SetCurrentMove(PossibleMoves.TurningLeft);
        }

        /// <summary>
        /// Calling this method will turn the player 90 degrees to the right.
        /// </summary>
        public void TurnRight()
        {
            SetCurrentMove(PossibleMoves.TurningRight);
        }

        /// <summary>
        /// Calling this method will turn the player 180 degrees around.
        /// </summary>
        public void TurnAround()
        {
            SetCurrentMove(PossibleMoves.TurningAround);
        }

        /// <summary>
        /// Calling this method will self destruct the player resulting in its death.
        /// </summary>
        public void SelfDestruct()
        {
            SetCurrentMove(PossibleMoves.SelfDestruct);
        }

        /// <summary>
        /// Calling this method will execute a melee attack.
        /// </summary>
        public void MeleeAttack()
        {
            SetCurrentMove(PossibleMoves.MeleeAttack);
        }

        /// <summary>
        /// Calling this method will execute a ranged attack to the specified location.
        /// </summary>
        public void RangedAttack(Int32 x, Int32 y)
        {
            SetCurrentMove(PossibleMoves.RangedAttack, x, y);
        }

        /// <summary>
        /// Calling this method will teleport the player to the specified location.
        /// </summary>
        public void Teleport(Int32 x, Int32 y)
        {
            SetCurrentMove(PossibleMoves.Teleport, x, y);
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

        private Boolean SetCurrentMove(PossibleMoves currentMove)
        {
            if (_50437079C366407D978Fe4Afd60C535F.CurrentMove == PossibleMoves.Idling)
            {
                _50437079C366407D978Fe4Afd60C535F.CurrentMove = currentMove;
                return true;
            }

            return false;
        }

        private void SetCurrentMove(PossibleMoves currentMove, Int32 x, Int32 y)
        {
            if (SetCurrentMove(currentMove))
            {
                _50437079C366407D978Fe4Afd60C535F.MoveDestinationX = x;
                _50437079C366407D978Fe4Afd60C535F.MoveDestinationY = y;
            }
        }

        #endregion
    }
}