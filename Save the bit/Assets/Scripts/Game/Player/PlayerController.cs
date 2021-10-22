using System;
using Game.Missiles;
using UnityEngine;

namespace Game.Player
{
    public sealed class PlayerController : MonoBehaviour
    {
        public event Action<PlayerController, CosmicRay> CosmicRayHit;
        public event Action<PlayerController, MissileBasic> MissileHit;
        public event Action CoinHit;

        public Vector3 Velocity => _rigidbody.velocity;

        public float minRayStrength = 10; // only rays above this strength can affect player
        [SerializeField] private RigidbodyController controller;

        internal InputManager InputManager;
        internal Explosion DeadExplosion;
        
        private Rigidbody2D _rigidbody;

        internal float XMultiplier = 1, YMultiplier = 1;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            controller.rigidbody = _rigidbody;
        }

        private void FixedUpdate()
        {
            if (InputManager == null)
            {
                controller.Update(Vector2.zero);
                return;
            }
            
            var dir = InputManager.GetMoveDirection();
            dir.x *= XMultiplier;
            dir.y *= YMultiplier;
            controller.Update(dir);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.IsPartOfLayer("CosmicRay"))
            {
                var ray = other.GetComponent<CosmicRay>();
                if (ray.strength >= minRayStrength)
                {
                    OnCosmicRayHit(this, ray);
                }
            } else if (other.IsPartOfLayer("Missile"))
            {
                DeadExplosion.ExplodeAt(transform.position);
                OnMissileHit(this, other.GetComponent<MissileBasic>());
            } else if (other.IsPartOfLayer("Coin"))
            {
                OnCoinHit();
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

        private void OnCoinHit()
        {
            CoinHit?.Invoke();
        }
    }
}