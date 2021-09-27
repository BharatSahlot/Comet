using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Missiles
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Missiles/Basic")]
    public class Basic : MonoBehaviour
    {
        public enum ModificationType
        {
            None,
            InvertX,
            InvertY,
            InvertXY,
            ChangeTarget,
            SelfDestruct
        }

        [Serializable]
        public class Modification
        {
            public ModificationType modificationType;
            [Range(0, 1)] public float probability;
        }

        [SerializeField] private float lifeTime;
        [SerializeField] private float randomLifeTimeDelta;
        [SerializeField] private float dieAnimationTime;
        [SerializeField] private RigidbodyController controller;
        [SerializeField] private TrailRenderer trail;
        [SerializeField] private Modification[] modifications;
        [SerializeField] private Explosion explosion;
        
        private Rigidbody2D _rigidbody;
        private GameObject _player;
        
        // modification helpers
        private GameObject _target;
        private float _xMultiplier = 1;
        private float _yMultiplier = 1;

        private float _lifeTime;
        private float _elapsed;
        private ModificationType _currentModification;

        private Vector3 _defaultScale;
        private bool _dead = false;

        private bool _playDeadExplosion = true;

        public Pool<Basic> Pool;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            controller.rigidbody = _rigidbody;
            _player = GameObject.FindWithTag("Player");
            _target = _player;

            _defaultScale = transform.localScale;
        }

        private void OnEnable()
        {
            _playDeadExplosion = true;
            _lifeTime = lifeTime + Random.Range(-1, 1) * randomLifeTimeDelta;
            _xMultiplier = _yMultiplier = 1;
            _elapsed = 0;
            _dead = false;
            _currentModification = ModificationType.None;
        }

        private void Update()
        {
            _elapsed += Time.deltaTime;
            if (_elapsed >= _lifeTime)
            {
                var elapsed = _elapsed - _lifeTime;
                var t =  elapsed / dieAnimationTime;
                transform.localScale = _defaultScale * Mathf.Lerp(1, 0, t);
                trail.widthMultiplier = Mathf.Lerp(1, 0, t);
                
                _dead = true;
                if(elapsed >= dieAnimationTime) Pool.Return(this);
            }
        }

        private void FixedUpdate()
        {
            if (_dead)
            {
                _rigidbody.drag = 0.5f;
                return;
            }

            var dir = transform.up;
            if (_target != null)
            {
                dir = (_target.transform.position - transform.position).normalized;
                dir.x *= _xMultiplier;
                dir.y *= _yMultiplier;
            }
            controller.Update(dir);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("CosmicRay"))
            {
                if (_currentModification != ModificationType.None)
                {
                    _currentModification = ModificationType.None;
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
                            _currentModification = modification.modificationType;
                            break;
                        }
                    }
                }

                ApplyModification(_currentModification);
                // } else if ((other.gameObject.layer & LayerMask.NameToLayer("Player")) != 0)
            } else if (LayerMask.NameToLayer("Missile") == other.gameObject.layer)
            {
                // play mini explosion
                if (_playDeadExplosion)
                {
                    other.GetComponent<Basic>()._playDeadExplosion = false;
                    var go = Instantiate(explosion);
                    go.ExplodeAt(transform.position);
                }
                Pool.Return(this);
            } else if(LayerMask.NameToLayer("Player") == other.gameObject.layer)
            {
                Pool.Return(this);
            }
        }

        private void ApplyModification(ModificationType modificationType)
        {
            switch (modificationType)
            {
                case ModificationType.None:
                    _xMultiplier = _yMultiplier = 1;
                    _target = _player;
                    break;
                case ModificationType.InvertX:
                    _xMultiplier = -1;
                    break;
                case ModificationType.InvertY:
                    _yMultiplier = -1;
                    break;
                case ModificationType.InvertXY:
                    _xMultiplier = -1;
                    _yMultiplier = -1;
                    break;
                case ModificationType.ChangeTarget:
                    var active = Pool.Active;
                    if (active.Count == 0)
                    {
                        _target = null;
                    }
                    else
                    {
                        var random = Random.Range(0, active.Count);
                        _target = active[random].gameObject;
                    }
                    break;
                case ModificationType.SelfDestruct:
                    Pool.Return(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modificationType), modificationType, null);
            }
        }
    }
}