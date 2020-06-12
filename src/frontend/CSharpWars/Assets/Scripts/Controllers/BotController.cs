using System;
using System.Threading.Tasks;
using Adic;
using Assets.Scripts.Helpers;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class BotController : BaseBehaviour
    {
        #region <| Dependencies |>

        [Inject]
        private IGameState _gameState;

        [Inject("robot-head")]
        public Transform Head;

        [Inject("prefab-explosion")]
        private GameObject ExplosionPrefab;

        [Inject("prefab-error")]
        private GameObject ErrorPrefab;
        
        [Inject("prefab-ranged-attack")]
        private GameObject RangedAttackPrefab;

        #endregion

        #region <| Private Members |>

        private Bot _bot;
        private Animation _animation;

        private string _lastAnimation;

        private GameObject _errorGameObject;
        private GameObject _rangedAttackGameObject;
        private bool _rangeAttackExecuted;
        private bool _died;

        public float Speed = 1.5f;
        public float RotationSpeed = 2.5f;
        

        private Guid _botId;

        public void SetBotId(Guid botId)
        {
            _botId = botId;
        }

        #endregion

        public async override Task Start()
        {
            await base.Start();

            InstantRefresh();

            _gameState.BotShouldBeUpdated.AddListener(OnBotShouldBeUpdated);
            _gameState.BotShouldBeRemoved.AddListener(OnBotShouldBeRemoved);

            _animation = gameObject.GetComponentInChildren<Animation>();
            if (_animation != null)
            {
                _animation[Animations.Walk].speed = Speed * 2;
                _animation[Animations.Turn].speed = Speed * 2;
                _animation[Animations.Jump].speed = Speed;
            }
        }

        #region <| Event Handlers |>

        private void OnBotShouldBeUpdated(Guid botId)
        {
            if( botId == _botId )
            {
                _bot = _gameState[botId];
                _rangeAttackExecuted = false;
            }            
        }

        private void OnBotShouldBeRemoved(Guid botId)
        {
            if(botId == _botId)
            {
                Destroy(gameObject);
                Destroy(this);
            }            
        }

        #endregion

        void Update()
        {
            if( _bot == null || _died )
            {
                return;
            }

            if ( _bot.Move == PossibleMoves.ScriptError )
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
            Vector3 targetWorldPosition = _gameState.ArenaToWorldPosition(_bot.X, _bot.Y);
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

            if (_bot.CurrentHealth <= 0 && _bot.Move != PossibleMoves.SelfDestruct)
            {
                _died = true;
                RunAnimationOnce(Animations.Death);
                return;
            }

            if (_bot.Move == PossibleMoves.MeleeAttack)
            {
                RunAnimationOnce(Animations.MeleeAttack);
                return;
            }

            if (_bot.Move == PossibleMoves.RangedAttack)
            {
                if (!_rangeAttackExecuted)
                {
                    _rangeAttackExecuted = true;
                    _rangedAttackGameObject = Instantiate(RangedAttackPrefab);
                    _rangedAttackGameObject.transform.SetParent(transform);
                    var rangedAttackController = _rangedAttackGameObject.GetComponent<RangedAttackController>();
                    Vector3 startPos = _gameState.ArenaToWorldPosition(_bot.X, _bot.Y);
                    Vector3 targetPos = _gameState.ArenaToWorldPosition(_bot.LastAttackX, _bot.LastAttackY);
                    rangedAttackController.Fire(startPos, targetPos);
                    RunAnimation(Animations.RangedAttack);
                }
                return;
            }

            if (_bot.Move == PossibleMoves.SelfDestruct)
            {
                GetComponent<ExplosionController>().Explode();
                RunAnimationOnce(Animations.Death);
                _died = true;
                return;
            }

            RunAnimation(Animations.Idle);
        }

        private void RunAnimation(string animationName)
        {
            if (!_animation.IsPlaying(animationName))
            {
                _animation.Stop();
                _animation.Play(animationName);
                _lastAnimation = null;
            }
        }

        private void RunAnimationOnce(string animationName)
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

        private void InstantRefresh()
        {
            var bot = _gameState[_botId];
            transform.position = _gameState.ArenaToWorldPosition(bot.X, bot.Y);
            transform.eulerAngles = OrientationVector.CreateFrom(bot.Orientation);
            _lastAnimation = null;
        }
    }
}