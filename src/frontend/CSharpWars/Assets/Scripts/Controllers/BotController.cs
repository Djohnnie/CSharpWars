using System;
using Assets.Scripts.Helpers;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class BotController : MonoBehaviour
    {
        private Bot _bot;
        private Animation _animation;
        private ArenaController _arenaController;



        public Single Speed = 1;
        public Single RotationSpeed = 2;

        private String _lastAnimation;

        void Start()
        {
            _animation = gameObject.GetComponentInChildren<Animation>();
            if (_animation != null)
            {
                _animation[Animations.Walk].speed = Speed * 2;
            }
            InstantRefresh();
        }

        void Update()
        {
            //if (BotIsNotAvailable())
            //{
            //    return;
            //}

            //if (RobotIsConfused())
            //{
            //    RunAnimationOnce(Animations.Defend);
            //    return;
            //    //if (!_exclamationMarkVisible)
            //    //{
            //    //    exclamationMark = Instantiate(exclamationMarkPrefab);
            //    //    exclamationMark.transform.SetParent(head);
            //    //    exclamationMark.transform.localPosition = new Vector3(0, 0, 0);
            //    //    exclamationMark.transform.position = new Vector3(exclamationMark.transform.position.x, 2.5f, exclamationMark.transform.position.z);
            //    //    _exclamationMarkVisible = true;
            //    //}
            //}
            //else
            //{
            //    //_exclamationMarkVisible = false;
            //    //if (exclamationMark != null) { Destroy(exclamationMark); }
            //}

            //if (_bot.Move == PossibleMoves.WalkForward)
            //{
            //    Single step = _bot.Move == PossibleMoves.Teleport ? 1 : Speed * Time.deltaTime;
            //    Vector3 targetWorldPosition = _arenaController.ArenaToWorldPosition(_bot.X, _bot.Y);
            //    Vector3 newPos = Vector3.MoveTowards(transform.position, targetWorldPosition, step);
            //    if ((newPos - transform.position).magnitude > 0.01)
            //    {
            //        RunAnimation(Animations.Walk);
            //        transform.position = newPos;
            //        return;
            //    }
            //}

            //if (_bot.Move == PossibleMoves.TurningLeft || _bot.Move == PossibleMoves.TurningRight ||
            //    _bot.Move == PossibleMoves.TurningAround)
            //{
            //    Vector3 targetOrientation = OrientationVector.CreateFrom(_bot.Orientation);
            //    Single rotationStep = RotationSpeed * Time.deltaTime;
            //    Vector3 newDir = Vector3.RotateTowards(transform.forward, targetOrientation, rotationStep, 0.0F);
            //    if (targetOrientation != newDir)
            //    {
            //        transform.rotation = Quaternion.LookRotation(newDir);
            //    }
            //    if ((targetOrientation - newDir).magnitude > 0.01)
            //    {
            //        RunAnimation(Animations.TurnRight);
            //        return;
            //    }
            //}

            //if (RobotHasDied())
            //{
            //    RunAnimationOnce(Animations.Death);
            //    return;
            //}

            //if (RobotIsAttackingUsingMelee())
            //{
            //    RunAnimationOnce(Animations.MeleeAttack);
            //    return;
            //}

            //if (RobotIsAttackingUsingRanged())
            //{
            //    //if (rangeAttack == null && !_rangeAttackFired)
            //    //{
            //    //    _rangeAttackFired = true;
            //    //    rangeAttack = Instantiate(rangeAttackPrefab);
            //    //    //rangeAttack.transform.SetParent(this.transform);
            //    //    RangeAttackController rangeAttackController = rangeAttack.GetComponent<RangeAttackController>();
            //    //    Vector3 startPos = GridController.Instance.gridToWorldPosition(_bot.Location.X, _bot.Location.Y);
            //    //    Vector3 targetPos = GridController.Instance.gridToWorldPosition(_bot.LastAttackLocation.X, _bot.LastAttackLocation.Y);
            //    //    rangeAttackController.fire(startPos, targetPos, 3f);
            //    //    GoAnim(RANGED_ATTACK);
            //    //}
            //    RunAnimationOnce(Animations.RangedAttack);
            //    return;
            //}

            //if (RobotIsSelfDestructing())
            //{
            //    //GetComponent<ExploderController>().Do(x => x.Explode());
            //    RunAnimationOnce(Animations.Death);
            //    return;
            //}

            //if (RobotIsTeleporting())
            //{
            //    RunAnimationOnce(Animations.Jump);
            //    return;
            //}

            //RunAnimation(Animations.Idle);

            if (_bot != null)
            {
                if (_bot.Move != PossibleMoves.ScriptError)
                {
                    //_exclamationMarkVisible = false;
                    //if (exclamationMark != null) { Destroy(exclamationMark); }
                }

                if (_bot.CurrentHealth <= 0 && _bot.Move != PossibleMoves.SelfDestruct)
                {
                    RunAnimationOnce(Animations.Death);
                }
                else
                {
                    Vector3 targetDir = OrientationVector.CreateFrom(_bot.Orientation);
                    float rotationStep = RotationSpeed * Time.deltaTime;
                    Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, rotationStep, 0.0F);
                    if (targetDir != newDir)
                    {
                        transform.rotation = Quaternion.LookRotation(newDir);
                    }

                    float step = _bot.Move == PossibleMoves.Teleport ? 1 : Speed * Time.deltaTime;
                    Vector3 targetWorldPosition = _arenaController.ArenaToWorldPosition(_bot.X, _bot.Y);
                    Vector3 newPos = Vector3.MoveTowards(transform.position, targetWorldPosition, step);
                    if ((targetDir - newDir).magnitude > 0.01)
                    {
                        RunAnimation(Animations.TurnRight);
                    }
                    else if ((newPos - transform.position).magnitude > 0.01)
                    {
                        RunAnimation(Animations.Walk);
                    }
                    else
                    {
                        switch (_bot.Move)
                        {
                            case PossibleMoves.MeleeAttack:
                                RunAnimation(Animations.MeleeAttack);
                                break;
                            case PossibleMoves.RangedAttack:
                                //if (rangeAttack == null && !_rangeAttackFired)
                                //{
                                //    _rangeAttackFired = true;
                                //    rangeAttack = Instantiate(rangeAttackPrefab);
                                //    //rangeAttack.transform.SetParent(this.transform);
                                //    RangeAttackController rangeAttackController = rangeAttack.GetComponent<RangeAttackController>();
                                //    Vector3 startPos = GridController.Instance.gridToWorldPosition(_bot.Location.X, _bot.Location.Y);
                                //    Vector3 targetPos = GridController.Instance.gridToWorldPosition(_bot.LastAttackLocation.X, _bot.LastAttackLocation.Y);
                                //    rangeAttackController.fire(startPos, targetPos, 3f);
                                //    RunAnimation(RANGED_ATTACK);
                                //}
                                break;
                            case PossibleMoves.SelfDestruct:
                                //GetComponent<ExploderController>().Do(x => x.Explode());
                                RunAnimationOnce(Animations.Death);
                                break;
                            case PossibleMoves.ScriptError:
                                //if (!_exclamationMarkVisible)
                                //{
                                //    exclamationMark = Instantiate(exclamationMarkPrefab);
                                //    exclamationMark.transform.SetParent(head);
                                //    exclamationMark.transform.localPosition = new Vector3(0, 0, 0);
                                //    exclamationMark.transform.position = new Vector3(exclamationMark.transform.position.x, 2.5f, exclamationMark.transform.position.z);
                                //    _exclamationMarkVisible = true;
                                //}
                                break;
                            default:
                                RunAnimation(Animations.Idle);
                                break;
                        }
                    }
                    transform.position = newPos;

                    Debug.DrawRay(transform.position, newDir, Color.red);
                }
            }
        }

        void RunAnimation(String animationName)
        {
            if (!_animation.IsPlaying(animationName))
            {
                _animation.Stop();
                _animation.Play(animationName);
                _lastAnimation = null;
            }
        }

        void RunAnimationOnce(String animationName)
        {
            if (!_animation.IsPlaying(animationName) && _lastAnimation != animationName)
            {
                _animation.Stop();
                _animation.Play(animationName);
                _lastAnimation = animationName;
            }
        }

        public void SetBot(Bot bot)
        {
            _bot = bot;
        }

        public void SetArenaController(ArenaController arenaController)
        {
            _arenaController = arenaController;
        }

        public void InstantRefresh()
        {
            if (_bot != null)
            {
                transform.position = _arenaController.ArenaToWorldPosition(_bot.X, _bot.Y);
                transform.eulerAngles = OrientationVector.CreateFrom(_bot.Orientation);
                _lastAnimation = null;
            }
        }








        private Boolean BotIsNotAvailable()
        {
            return _bot == null;
        }

        private Boolean RobotIsConfused()
        {
            return _bot.Move == PossibleMoves.ScriptError;
        }

        private Boolean RobotHasDied()
        {
            return _bot.CurrentHealth <= 0 && _bot.Move != PossibleMoves.SelfDestruct;
        }

        private Boolean RobotIsAttackingUsingMelee()
        {
            return _bot.Move == PossibleMoves.MeleeAttack;
        }

        private Boolean RobotIsAttackingUsingRanged()
        {
            return _bot.Move == PossibleMoves.RangedAttack;
        }

        private Boolean RobotIsSelfDestructing()
        {
            return _bot.Move == PossibleMoves.SelfDestruct;
        }

        private Boolean RobotIsTeleporting()
        {
            return _bot.Move == PossibleMoves.Teleport;
        }
    }
}