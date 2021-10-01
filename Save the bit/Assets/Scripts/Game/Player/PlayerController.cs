using System;
using Game.Missiles;
using UnityEngine;

namespace Game.Player
{
    public sealed class PlayerController : MonoBehaviour
    {
        public event Action<PlayerController, CosmicRay> CosmicRayHit;
        public event Action<PlayerController, MissileBasic> MissileHit;

        public Vector3 Velocity => _rigidbody.velocity;

        [SerializeField] private RigidbodyController controller;

        internal GameObject Shield;
        private Rigidbody2D _rigidbody;

        internal float XMultiplier = 1, YMultiplier = 1;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            controller.rigidbody = _rigidbody;
        }

        private void FixedUpdate()
        {
            var dir = InputManager.GetMoveDirection();
            dir.x *= XMultiplier;
            dir.y *= YMultiplier;
            controller.Update(dir);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.IsPartOfLayer("CosmicRay"))
            {
                OnCosmicRayHit(this, other.GetComponent<CosmicRay>());
            } else if (other.IsPartOfLayer("Missile"))
            {
                OnMissileHit(this, other.GetComponent<MissileBasic>());
            }
        }

        private void OnDestroy()
        {
            CosmicRayHit = null;
            MissileHit = null;
        }

        private void OnCosmicRayHit(PlayerController arg1, CosmicRay arg2)
        {
            CosmicRayHit?.Invoke(arg1, arg2);
        }

        private void OnMissileHit(PlayerController arg1, MissileBasic arg2)
        {
            MissileHit?.Invoke(arg1, arg2);
        }
    }
}