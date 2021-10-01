using System.Collections;
using Game.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class CosmicRaySpawner : MonoBehaviour
    {
        [SerializeField] private float spawnDistance = 50;
        [SerializeField] private float spawnDelay = 1; // wait this much sec before spawning new
        [SerializeField] private CosmicRay prefab;

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
            
                var playerPos = (Player.transform.position + Player.Velocity * (spawnDistance / prefab.maxSpeed)).xy();
                var position = Player.transform.position.xy() + Random.insideUnitCircle.normalized * spawnDistance;
                var lookDir = playerPos - position;
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