using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private Rigidbody2D shield;

    private Rigidbody2D _rigidbody;
    
    public float speed = 10;
    public float maxRot = 5;
    public float maxGRot = 2;
    public float shieldAngle = 60;

    public Vector3 Velocity => _rigidbody.velocity;

    private Vector2 _up = Vector2.up;
    private Vector2 _currentVelocity;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    public void AddGravity(Vector2 origin, float mass)
    {
        var dir = -(_rigidbody.position - origin);
        var force = _rigidbody.mass * mass / dir.sqrMagnitude;
        _rigidbody.AddForce(force * dir.normalized, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        var dir = new Vector3(joystick.X, joystick.Y).normalized;
        var angle = Mathf.Abs(Vector2.SignedAngle(_up, dir));
        _up = Vector2.SmoothDamp(_up, dir, ref _currentVelocity, angle / (maxRot), maxRot * Time.fixedDeltaTime).normalized;

        _rigidbody.AddForce(_up * speed * Time.fixedDeltaTime, ForceMode2D.Impulse);

        dir = _rigidbody.velocity.normalized;
        angle = Vector2.SignedAngle(Vector2.up, dir);
        var rot = Mathf.MoveTowardsAngle(_rigidbody.rotation, angle, maxGRot * Time.fixedDeltaTime);
        _rigidbody.MoveRotation(rot);

        // shield.velocity = _rigidbody.velocity;
        // shield.position = _rigidbody.position;
        // shield.rotation = _rigidbody.rotation;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.layer & LayerMask.NameToLayer("CosmicRay")) == 0) return;
    }
}
