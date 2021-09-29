using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Missiles
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Missiles/Basic")]
    public class Basic : MonoBehaviour
    {
        [SerializeField] private float lifeTime;
        [SerializeField] private float randomLifeTimeDelta;
        [SerializeField] private float dieAnimationTime;
        [SerializeField] private RigidbodyController controller;
        [SerializeField] private TrailRenderer trail;
        [SerializeField] private Modification[] modifications;
        [SerializeField] private Explosion explosion;
        [SerializeField] private GameObject offScreenIcon;
        [SerializeField] private float screenBorder;

        private Rigidbody2D _rigidbody;
        private GameObject _player;
        private SpriteRenderer _renderer;
        
        // modification helpers
        private GameObject _target;
        private float _xMultiplier = 1;
        private float _yMultiplier = 1;

        private float _lifeTime;
        private float _elapsed;
        private ModificationType _currentModificationType;

        private Vector3 _defaultScale;
        private bool _dead = false;

        private bool _playDeadExplosion = true;

        private GameObject _icon;
        private Camera _camera;

        public Pool<Basic> Pool;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();
            controller.rigidbody = _rigidbody;
            _player = GameObject.FindWithTag("Player");
            _target = _player;

            _camera = Camera.main;
            
            _icon = Instantiate(offScreenIcon);
            _icon.SetActive(false);

            _defaultScale = transform.localScale;
        }

        private void OnEnable()
        {
            _playDeadExplosion = true;
            _lifeTime = lifeTime + Random.Range(-1, 1) * randomLifeTimeDelta;
            _xMultiplier = _yMultiplier = 1;
            _elapsed = 0;
            _dead = false;
            _currentModificationType = ModificationType.None;
        }

        private void OnDisable()
        {
            if (_icon != null)
            {
                _icon.SetActive(false);
            }
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
                _icon.SetActive(false);
                
                _dead = true;
                if(elapsed >= dieAnimationTime) Pool.Return(this);
            }
        }

        private void LateUpdate()
        {
            PositionIcon();
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
            
            // PositionIcon();
        }

        private void PositionIcon()
        {
            if (!_dead && !_renderer.isVisible && _player != null)
            {
                if(!_icon.activeSelf) _icon.SetActive(true);
            
                var corners = new Vector3[4]
                {
                    _camera.ViewportToWorldPoint(new Vector3(0, 0, 0)),
                    _camera.ViewportToWorldPoint(new Vector3(0, 1, 0)),
                    _camera.ViewportToWorldPoint(new Vector3(1, 0, 0)),
                    _camera.ViewportToWorldPoint(new Vector3(1, 1, 0)),
                };
            
                float minX = float.MaxValue, maxX = float.MinValue;
                float minY = float.MaxValue, maxY = float.MinValue;
                foreach (var localCorner in corners)
                {
                    var corner = _camera.transform.TransformVector(localCorner);
                    minX = Mathf.Min(minX, corner.x);
                    minY = Mathf.Min(minY, corner.y);
                    maxX = Mathf.Max(maxX, corner.x);
                    maxY = Mathf.Max(maxY, corner.y);
                }
            
                minX += screenBorder;
                maxX -= screenBorder;
                minY += screenBorder;
                maxY -= screenBorder;
            
                var pos = transform.position;
                pos.x = Mathf.Clamp(pos.x, minX, maxX);
                pos.y = Mathf.Clamp(pos.y, minY, maxY);
                _icon.transform.position = pos;
            
                _icon.transform.up = (_player.transform.position - pos).normalized;
            }
            else
            {
                if(_icon.activeSelf) _icon.SetActive(false);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("CosmicRay"))
            {
                if (_currentModificationType != ModificationType.None)
                {
                    _currentModificationType = ModificationType.None;
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
                            _currentModificationType = modification.modificationType;
                            break;
                        }
                    }
                }

                ApplyModification(_currentModificationType);
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
                    var go = Instantiate(explosion);
                    go.ExplodeAt(transform.position);
                    Pool.Return(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modificationType), modificationType, null);
            }
        }
    }
}