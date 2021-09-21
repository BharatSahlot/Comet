using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CosmicRaySpawner : MonoBehaviour
{
    [SerializeField] private float spawnDistance = 50;
    [SerializeField] private float spawnDelay = 1; // wait this much sec before spawning new
    [SerializeField] private PlayerController player;
    [SerializeField] private CosmicRay prefab;

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
            var playerPos = (player.transform.position + player.Velocity * (spawnDistance / prefab.maxSpeed)).xy();
            var position = player.transform.position.xy() + Random.insideUnitCircle.normalized * spawnDistance;
            var lookDir = playerPos - position;
            // var ray = Instantiate(prefab, position, prefab.transform.rotation);
            var ray = _pool.Borrow(false);
            ray.transform.position = position;
            ray.gameObject.name = "Cosmic Ray";
            ray.transform.rotation = Quaternion.LookRotation(Vector3.forward, lookDir.normalized);
            ray.pool = _pool;
            ray.gameObject.SetActive(true);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}