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