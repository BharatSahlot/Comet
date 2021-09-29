using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private Rigidbody2D shield;
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private Transform sprite;
    [SerializeField] private Explosion explosion;

    [SerializeField] private RigidbodyController controller;
    [SerializeField] private Modification[] modifications;

    private Rigidbody2D _rigidbody;

    private Modification _currentModification = null;
    private float _xMultiplier = 1;
    private float _yMultiplier = 1;
    
    public Vector3 Velocity => _rigidbody.velocity;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        controller.rigidbody = _rigidbody;
    }
    
    public void AddGravity(Vector2 origin, float mass)
    {
        var dir = -(_rigidbody.position - origin);
        var force = _rigidbody.mass * mass / dir.sqrMagnitude;
        _rigidbody.AddForce(force * dir.normalized, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        var dir = new Vector2(joystick.X * _xMultiplier, joystick.Y * _yMultiplier).normalized;
        controller.Update(dir);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CosmicRay"))
        {
            if (_currentModification != null)
            {
                ApplyModification(null);
            }
            else
            {
                var random = Random.Range(0f, 1f);
                float sum = 0;
                foreach (var modification in modifications)
                {
                    sum += modification.probability;
                    if (random <= sum)
                    {
                        // apply modification
                        ApplyModification(modification);
                        break;
                    }
                }
            }
            timeManager.PlayerHitRay();
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            explosion.ExplodeAt(transform.position);
            timeManager.PlayerHitMissile();
            
            joystick = null;
            sprite = null;
            explosion = null;
            controller = null;
            _rigidbody = null;
            Destroy(shield.gameObject);
            shield = null;
            Destroy(gameObject);
        }
    }

    private void ApplyModification(Modification modification)
    {
        if (_currentModification?.playerEffectPanelUI)
        {
            _currentModification.playerEffectPanelUI.Hide();
        }

        _currentModification = modification;
        if (modification == null)
        {
            _currentModification = null;
            shield.gameObject.SetActive(true);
            _xMultiplier = _yMultiplier = 1;
            return;
        }
        if (_currentModification.playerEffectPanelUI)
        {
            _currentModification.playerEffectPanelUI.ShowForSeconds(10);
        }
        switch (modification.modificationType)
        {
            case ModificationType.InvertX:
                _xMultiplier *= -1;
                break;
            case ModificationType.InvertY:
                _yMultiplier *= -1;
                break;
            case ModificationType.InvertXY:
                _xMultiplier = -1;
                _yMultiplier = -1;
                break;
            case ModificationType.TurnOffShield:
                shield.gameObject.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
