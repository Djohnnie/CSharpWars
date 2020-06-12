using System;
using System.Collections.Generic;
using Adic;
using Assets.Scripts.Controllers;
using Assets.Scripts.Helpers;
using Assets.Scripts.Networking;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class Prefabs
    {
        public GameObject BotPrefab;

        public GameObject NameTagPrefab;

        public GameObject HealthTagPrefab;

        public GameObject StaminaTagPrefab;

        public GameObject ExplosionPrefab;

        public GameObject ErrorPrefab;

        public GameObject RangedAttackPrefab;
    }

    public class GameRoot : ContextRoot
    {
        #region <| Inspector Properties |>

        public GameObject game;
        public GameObject camera;
        public GameObject floor;
        public GameObject arena;
        public Prefabs Prefabs;

        #endregion

        #region <| ContextRoot Overrides |>

        public override void SetupContainers()
        {
            AddContainer<InjectionContainer>()   
                .Group("Dependency Injection", c =>
                {
                    c.Bind<IApiClient>().To<ApiClient>();
                    c.Bind<IGameState>().ToSingleton<GameState>();
                    //c.Bind<IMove>().To<Move1>();
                    //c.Bind<IMove>().To<Move2>();
                    //c.Bind<IMoveProcessor>().To<MoveProcessor>();
                })
                .Group("Floor", c =>
                {
                    c.Bind<GameObject>().To(floor).As("floor");
                    c.Bind<Renderer>().To(floor.GetComponent<Renderer>()).As("floor-renderer");

                    c.Bind<Transform>().To(FindDeepChild(Prefabs.BotPrefab.transform,"robo_rigg_2014:head")).As("robot-head");
                })
                .Group("Prefabs", c =>
                {
                    c.Bind<BotController>().ToGameObject(Prefabs.BotPrefab);
                    c.Bind<NameTagController>().ToGameObject(Prefabs.NameTagPrefab);
                    c.Bind<HealthTagController>().ToGameObject(Prefabs.HealthTagPrefab);
                    c.Bind<StaminaTagController>().ToGameObject(Prefabs.StaminaTagPrefab);
                    c.Bind<ExplosionController>().ToGameObject(Prefabs.BotPrefab);
                    c.Bind<RangedAttackController>().ToGameObject(Prefabs.RangedAttackPrefab);
                })
                .Group("Prefabs", c =>
                {
                    c.Bind<GameObject>().To(Prefabs.BotPrefab).As("prefab-bot");
                    c.Bind<GameObject>().To(Prefabs.NameTagPrefab).As("prefab-name-tag");
                    c.Bind<GameObject>().To(Prefabs.HealthTagPrefab).As("prefab-health-tag");
                    c.Bind<GameObject>().To(Prefabs.StaminaTagPrefab).As("prefab-stamina-tag");
                    c.Bind<GameObject>().To(Prefabs.ExplosionPrefab).As("prefab-explosion");
                    c.Bind<GameObject>().To(Prefabs.ErrorPrefab).As("prefab-error");
                    c.Bind<GameObject>().To(Prefabs.RangedAttackPrefab).As("prefab-ranged-attack");
                })                
                .Group("Controllers", c =>
                {
                    c.Bind<GameLoopController>().ToGameObject(game);
                    c.Bind<CameraController>().ToGameObject(camera);
                    c.Bind<ArenaController>().ToGameObject(arena);
                    c.Bind<BotsController>().ToGameObject(arena);
                });
        }

        public Transform FindDeepChild(Transform aParent, string aName)
        {
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(aParent);
            while (queue.Count > 0)
            {
                var c = queue.Dequeue();
                if (c.name == aName)
                    return c;
                foreach(Transform t in c)
                    queue.Enqueue(t);
            }
            return null;
        }

        public override void Init()
        {

        }

        #endregion
        
        #region <| Awake |>

        public new void Awake()
        {
            base.Awake();

    #if UNITY_EDITOR
            // Limit the FPS in the Unity Editor to 60.
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
    #endif
        }

        #endregion
    }
}