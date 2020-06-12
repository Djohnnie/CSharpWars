using System.Collections;
using System.Threading.Tasks;
using Adic;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class ExplosionController : BaseBehaviour
    {
        #region <| Dependencies |>

        [Inject("prefab-explosion")]
        private GameObject ExplosionPrefab;

        #endregion

        public float scale = 1;
        public float playbackSpeed = 1;
        public float explosionTime = 0;
        public float randomizeExplosionTime = 0;
        public float radius = 15;
        public float power = 1;
        public int probeCount = 150;
        public float explodeDuration = 0.5f;

        private bool exploded;

        public IEnumerator Explosion()
        {
            GameObject container = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            ParticleSystem[] systems = container.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem system in systems)
            {
                system.transform.localScale *= scale;
                var main = system.main;
                main.startSpeedMultiplier = scale;
                main.startSizeMultiplier = scale;
                main.simulationSpeed = playbackSpeed;
            }

            while (explodeDuration > Time.time - explosionTime)
            {
                for (int i = 0; i < probeCount; i++)
                {
                    ShootFromCurrentPosition();
                }
                yield return new WaitForFixedUpdate();
            }

            Destroy(container);
        }

        private void ShootFromCurrentPosition()
        {
            Vector3 probeDir = Random.onUnitSphere;
            Ray testRay = new Ray(transform.position, probeDir);
            ShootRay(testRay, radius);
        }

        public async override Task Start()
        {
            await base.Start();
            Init();
        }

        private void Init()
        {
            power *= 500000;

            if (randomizeExplosionTime > 0.01f)
            {
                explosionTime += Random.Range(0.0f, randomizeExplosionTime);
            }
        }

        public void Explode()
        {
            if (!exploded)
            {
                explosionTime = Time.time;
                exploded = true;
                StartCoroutine(nameof(Explosion));
            }
        }

        private void ShootRay(Ray testRay, float estimatedRadius)
        {
            RaycastHit hit;
            if (Physics.Raycast(testRay, out hit, estimatedRadius))
            {
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForceAtPosition(power * Time.deltaTime * testRay.direction / probeCount, hit.point);
                    estimatedRadius /= 2;
                }
                else
                {
                    Vector3 reflectVec = Random.onUnitSphere;
                    if (Vector3.Dot(reflectVec, hit.normal) < 0)
                    {
                        reflectVec *= -1;
                    }
                    Ray emittedRay = new Ray(hit.point, reflectVec);
                    ShootRay(emittedRay, estimatedRadius - hit.distance);
                }
            }
        }
    }
}