using System;
using UnityEngine;

[Serializable]
public class RigidbodyController
{
    public float speed = 100;
    public float smoothMoveRotation = 360;
    public float smoothLookRotation = 360;
    
    public bool IsRotating { get; private set; }
    
    [HideInInspector] public Rigidbody2D rigidbody;

    private Vector2 _moveDir = Vector2.up;
    private Vector2 _angularVelocity;

    public void Update(Vector2 input)
    {
        var angle = Mathf.Abs(Vector2.SignedAngle(_moveDir, input));
        _moveDir = Vector2.SmoothDamp(_moveDir, input, ref _angularVelocity, angle / (smoothMoveRotation), smoothMoveRotation * Time.fixedDeltaTime).normalized;
        
        rigidbody.AddForce(_moveDir * speed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        
        var dir = rigidbody.velocity.normalized;
        angle = Vector2.SignedAngle(Vector2.up, dir);
        var rot = Mathf.MoveTowardsAngle(rigidbody.rotation, angle, smoothLookRotation * Time.fixedDeltaTime);
        var delta = Mathf.DeltaAngle(rigidbody.rotation, rot);
        IsRotating = !Mathf.Approximately(delta, 0);
        
        rigidbody.MoveRotation(rot);
    }
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private Rigidbody2D shield;
    [SerializeField] private Transform sprite;
    [SerializeField] private Explosion explosion;

    [SerializeField] private RigidbodyController controller;

    private Rigidbody2D _rigidbody;
    
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
        var dir = new Vector2(joystick.X, joystick.Y).normalized;
        controller.Update(dir);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CosmicRay"))
        {
            Debug.Log("Hit by ray");
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            explosion.ExplodeAt(transform.position);
            
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
}
