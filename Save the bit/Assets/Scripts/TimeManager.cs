using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float playerHitRaySlowTimeDelay = 0f; // seconds
    [SerializeField] private float playerHitRaySlowTimeDuration = 2f; // seconds
    [SerializeField] private float playerHitRaySlowTimeScale = 0.5f; // time scale
    
    [SerializeField] private float playerHitMissileSlowTimeDelay = 0.5f; // seconds
    [SerializeField] private float playerHitMissileSlowTimeDuration = 2f; // seconds
    [SerializeField] private float playerHitMissileSlowTimeScale = 0.5f; // time scale

    private float _duration;
    private float _delay;
    private float _elapsed;
    private float _timeScale;
    
    public void PlayerHitRay()
    {
        _elapsed = 0;
        _delay = playerHitRaySlowTimeDelay;
        _duration = playerHitRaySlowTimeDuration;
        _timeScale = playerHitRaySlowTimeScale;
    }
    
    public void PlayerHitMissile()
    {
        _elapsed = 0;
        _delay = playerHitMissileSlowTimeDelay;
        _duration = playerHitMissileSlowTimeDuration;
        _timeScale = playerHitMissileSlowTimeScale;
    }

    private void Update()
    {
        _elapsed += Time.deltaTime;
        if (_elapsed >= _delay && _elapsed <= _delay + _duration)
        {
            Time.timeScale = _timeScale;
        } else if (_elapsed >= _delay + _duration)
        {
            _elapsed = _duration = _delay = 0;
            Time.timeScale = 1;
        }
    }
}