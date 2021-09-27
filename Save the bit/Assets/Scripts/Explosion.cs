using System;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public bool destroyOnFinish = false;
    private ParticleSystem[] _particles;

    private void Awake()
    {
        _particles = GetComponentsInChildren<ParticleSystem>();
        foreach (var system in _particles)
        {
            system.Stop();
        }
        gameObject.SetActive(false);
    }

    public void ExplodeAt(Vector3 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
        float duration = 0;
        foreach (var system in _particles)
        {
            system.Play();
            duration = Mathf.Max(duration, system.main.duration);
        }
        if(destroyOnFinish) Destroy(gameObject, duration);
    }
}