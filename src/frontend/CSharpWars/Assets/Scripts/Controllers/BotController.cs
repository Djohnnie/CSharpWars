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

        private string _lastAnimation;

        private GameObject _errorGameObject;
        private GameObject _rangedAttackGameObject;
        private bool _rangeAttackExecuted;
        private bool _died;

        public float Speed = 1;
        public float RotationSpeed = 2;

        public Transform Head;
        public GameObject ErrorPrefab;
        public GameObject RangedAttackPrefab;

        void Start()
        {
            _animation = gameObject.GetComponentInChildren<Animation>();
            if (_animation != null)
            {
                _animation[Animations.Walk].speed = Speed * 2;
                _animation[Animations.Turn].speed = Speed * 2;
                _animation[Animations.Jump].speed = Speed;
            }
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

            float step = Math.Abs(_bot.X - _bot.FromX) > 1 || Math.Abs(_bot.Y - _bot.FromY) > 1 ? 100 : Speed * Time.deltaTime;
            Vector3 targetWorldPosition = _arenaController.ArenaToWorldPosition(_bot.X, _bot.Y);
            Vector3 newPos = Vector3.MoveTowards(transform.position, targetWorldPosition, step);
            if ((newPos - transform.position).magnitude > 0.01)
            {
                RunAnimation(Animations.Walk);
                transform.position = newPos;
                return;
            }

            Vector3 targetOrientation = OrientationVector.CreateFrom(_bot.Orientation);
            float rotationStep = RotationSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetOrientation, rotationStep, 0.0F);
            if (targetOrientation != newDir)
            {
                transform.rotation = Quaternion.LookRotation(newDir);
            }

            if ((targetOrientation - newDir).magnitude > 0.01)
            {
                RunAnimation(Animations.Turn);
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
                _died = true;
                return;
            }

            RunAnimation(Animations.Idle);
        }

        void RunAnimation(string animationName)
        {
            if (!_animation.IsPlaying(animationName))
            {
                _animation.Stop();
                _animation.Play(animationName);
                _lastAnimation = null;
            }
        }

        void RunAnimationOnce(string animationName)
        {
            if (!_animation.IsPlaying(animationName) && _lastAnimation != animationName)
            {
                _animation.Stop();
                _animation.Play(animationName);
                _lastAnimation = animationName;
            }

            if (!_animation.IsPlaying(animationName))
            {
                _lastAnimation = null;
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
                _healthTagController.UpdateTag(bot);
            }

            if (_staminaTagController != null)
            {
                _staminaTagController.UpdateTag(bot);
            }

            _rangeAttackExecuted = false;
        }

        public void SetTagController(TagController tagController)
        {
            switch (tagController)
            {
                case HealthTagController t:
                    _healthTagController = t;
                    break;
                case StaminaTagController t:
                    _staminaTagController = t;
                    break;
                case NameTagController t:
                    _nameTagController = t;
                    break;
            }
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








        private bool BotIsNotAvailable()
        {
            return _bot == null || _died;
        }

        private bool RobotIsConfused()
        {
            return _bot.Move == PossibleMoves.ScriptError;
        }

        private bool RobotHasDied()
        {
            return _bot.CurrentHealth <= 0 && _bot.Move != PossibleMoves.SelfDestruct;
        }

        private bool RobotIsAttackingUsingMelee()
        {
            return _bot.Move == PossibleMoves.MeleeAttack;
        }

        private bool RobotIsAttackingUsingRanged()
        {
            return _bot.Move == PossibleMoves.RangedAttack;
        }

        private bool RobotIsSelfDestructing()
        {
            return _bot.Move == PossibleMoves.SelfDestruct;
        }

        private bool RobotIsTeleporting()
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