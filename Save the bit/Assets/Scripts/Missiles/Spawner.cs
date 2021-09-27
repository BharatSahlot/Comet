using System.Collections;
using UnityEngine;

namespace Missiles
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private float spawnDistance = 50;
        [SerializeField] private float spawnDelay = 1; // wait this much sec before spawning new
        [SerializeField] private PlayerController player;
        [SerializeField] private Basic prefab;

        private Pool<Basic> _pool;

        private void Awake()
        {
            _pool = new Pool<Basic>(prefab, 10);
        }

        private void Start()
        {
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                if (player == null) break;
                
                var position = player.transform.position.xy() + Random.insideUnitCircle.normalized * spawnDistance;
                var missile = _pool.Borrow(false);
                missile.transform.position = position;
                // ray.gameObject.name = "Cosmic Ray";
                missile.Pool = _pool;
                missile.gameObject.SetActive(true);
                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }
}