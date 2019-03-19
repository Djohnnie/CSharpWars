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
        private NameTagController _nameTagController;
        private HealthTagController _healthTagController;
        private StaminaTagController _staminaTagController;

        private String _lastAnimation;

        private GameObject _errorGameObject;
        private GameObject _rangedAttackGameObject;
        private Boolean _rangeAttackExecuted;
        private Boolean _died;

        public Single Speed = 1;
        public Single RotationSpeed = 2;

        public Transform Head;
        public GameObject ErrorPrefab;
        public GameObject RangedAttackPrefab;

        void Start()
        {
            _animation = gameObject.GetComponentInChildren<Animation>();
            if (_animation != null)
            {
                _animation[Animations.Walk].speed = Speed * 2;
                _animation[Animations.TurnRight].speed = Speed * 2;
                _animation[Animations.Jump].speed = Speed;
            }
            InstantRefresh();
        }

        void Update()
        {
            if (BotIsNotAvailable())
            {
                return;
            }

            if (RobotIsConfused())
            {
                RunAnimationOnce(Animations.Defend);
                if (_errorGameObject == null)
                {
                    _errorGameObject = Instantiate(ErrorPrefab);
                    _errorGameObject.transform.SetParent(Head);
                    _errorGameObject.transform.localPosition = new Vector3(0, 0, 0);
                    _errorGameObject.transform.position = new Vector3(_errorGameObject.transform.position.x, 2.5f, _errorGameObject.transform.position.z);
                }
                return;
            }

            if (_errorGameObject != null)
            {
                Destroy(_errorGameObject);
                _errorGameObject = null;
            }

            Single step = Math.Abs(_bot.X - _bot.FromX) > 1 || Math.Abs(_bot.Y - _bot.FromY) > 1 ? 100 : Speed * Time.deltaTime;
            Vector3 targetWorldPosition = _arenaController.ArenaToWorldPosition(_bot.X, _bot.Y);
            Vector3 newPos = Vector3.MoveTowards(transform.position, targetWorldPosition, step);
            if ((newPos - transform.position).magnitude > 0.01)
            {
                RunAnimation(Animations.Walk);
                transform.position = newPos;
                return;
            }

            Vector3 targetOrientation = OrientationVector.CreateFrom(_bot.Orientation);
            Single rotationStep = RotationSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetOrientation, rotationStep, 0.0F);
            if (targetOrientation != newDir)
            {
                transform.rotation = Quaternion.LookRotation(newDir);
            }

            if ((targetOrientation - newDir).magnitude > 0.01)
            {
                RunAnimation(Animations.TurnRight);
                return;
            }

            if (RobotHasDied())
            {
                _died = true;
                RunAnimationOnce(Animations.Death);
                _nameTagController.Destroy();
                _healthTagController.Destroy();
                _staminaTagController.Destroy();
                return;
            }

            if (RobotIsAttackingUsingMelee())
            {
                RunAnimationOnce(Animations.MeleeAttack);
                return;
            }

            if (RobotIsAttackingUsingRanged())
            {
                if (!_rangeAttackExecuted)
                {
                    _rangeAttackExecuted = true;
                    _rangedAttackGameObject = Instantiate(RangedAttackPrefab);
                    _rangedAttackGameObject.transform.SetParent(transform);
                    var rangedAttackController = _rangedAttackGameObject.GetComponent<RangedAttackController>();
                    Vector3 startPos = _arenaController.ArenaToWorldPosition(_bot.X, _bot.Y);
                    Vector3 targetPos = _arenaController.ArenaToWorldPosition(_bot.LastAttackX, _bot.LastAttackY);
                    rangedAttackController.Fire(startPos, targetPos);
                    RunAnimation(Animations.RangedAttack);
                }
                return;
            }

            if (RobotIsSelfDestructing())
            {
                GetComponent<ExplosionController>().Explode();
                RunAnimationOnce(Animations.Death);
                return;
            }

            RunAnimation(Animations.Idle);
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

        public void UpdateBot(Bot bot)
        {
            SetBot(bot);
            if (_healthTagController != null)
            {
                _healthTagController.UpdateBot(bot);
            }

            if (_staminaTagController != null)
            {
                _staminaTagController.UpdateBot(bot);
            }

            _rangeAttackExecuted = false;
        }

        public void SetNameTagController(NameTagController nameTagController)
        {
            _nameTagController = nameTagController;
        }

        public void SetHealthTagController(HealthTagController healthTagController)
        {
            _healthTagController = healthTagController;
        }

        public void SetStaminaTagController(StaminaTagController staminaTagController)
        {
            _staminaTagController = staminaTagController;
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
            return _bot == null || _died;
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

        public void Destroy()
        {
            if (_nameTagController != null)
            {
                _nameTagController.Destroy();
            }

            if (_healthTagController != null)
            {
                _healthTagController.Destroy();
            }

            if (_staminaTagController != null)
            {
                _staminaTagController.Destroy();
            }

            Destroy(gameObject);
            Destroy(this);
        }
    }
}