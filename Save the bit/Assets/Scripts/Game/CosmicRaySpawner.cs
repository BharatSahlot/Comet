using System.Collections;
using Game.Missiles;
using Game.Player;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class CosmicRaySpawner : MonoBehaviour
    {
        [SerializeField] private float spawnDistance = 50;
        [SerializeField] private float spawnDelay = 1; // wait this much sec before spawning new
        [SerializeField] private CosmicRay prefab;
        [SerializeField] private MissileSpawner missileSpawner;

        internal PlayerController Player;
        
        private Pool<CosmicRay> _pool;

        private void Awake()
        {
            _pool = new Pool<CosmicRay>(prefab, 10);
        }

        private void Start()
        {
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                if (Player == null) break;
                
                var target = Player.GetComponent<Rigidbody2D>();
                var rand = Random.value;
                if (rand >= 0.5 && missileSpawner.ActiveMissiles.Count > 0)
                {
                    target = missileSpawner.ActiveMissiles.GetRandom().GetComponent<Rigidbody2D>();
                }

                var currentTargetPosition = target.transform.position;
                var targetPosInFuture = (currentTargetPosition.xy() + target.velocity * (spawnDistance / prefab.maxSpeed));
                var position = currentTargetPosition.xy() + Random.insideUnitCircle.normalized * spawnDistance;
                var lookDir = targetPosInFuture - position;
                var ray = _pool.Borrow(false);
                ray.transform.position = position;
                ray.transform.rotation = Quaternion.LookRotation(Vector3.forward, lookDir.normalized);
                ray.Pool = _pool;
                ray.gameObject.SetActive(true);
                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }
}