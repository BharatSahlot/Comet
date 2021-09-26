using UnityEngine;

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
        
        private Rigidbody2D _rigidbody;
        private GameObject _player;

        private float _lifeTime;
        private float _elapsed;

        private Vector3 _defaultScale;
        private bool _dead = false;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            controller.rigidbody = _rigidbody;
            _player = GameObject.FindWithTag("Player");

            _lifeTime = lifeTime + Random.Range(-1, 1) * randomLifeTimeDelta;

            _defaultScale = transform.localScale;
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
                if(elapsed >= dieAnimationTime) Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            if (_dead)
            {
                _rigidbody.drag = 0.5f;
                return;
            }
            
            var dir = (_player.transform.position - transform.position).normalized;
            controller.Update(dir);
        }
    }
}