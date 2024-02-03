using System;
using System.Collections;
using Scripts.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Characters
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float _speed = 4f;
        [SerializeField] private int _pointValue = 10;
        [SerializeField] private Transform _laserBarrel;
        
        private Animator _anim;
        private Renderer _rend;
        private Collider _collider;
        private bool _isDead;
        private WaitForSeconds _delay = new WaitForSeconds(2.8f);

        public static Action onDamagePlayer;
        public static Action<GameObject> onResetEnemy;
        public static Action<int> onDestroyedEnemy;
        public static Action<int> onSendSFXEnemy;



        private void OnEnable()
        {
            SpawnManager.onActivateEnemy += ActivateEnemy;
        }

        private void Start()
        {
            _anim = GetComponentInChildren<Animator>();
            _rend = GetComponentInChildren<Renderer>();
            _collider = GetComponent<Collider>();

            if (_anim == null)
                Debug.LogError("Animator is Null");

            if (_rend == null)
                Debug.LogError("Renderer is NULL");

            if (_collider == null)
                Debug.LogError("Collider is NULL");
        }

        void Update()
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }

        private void ActivateEnemy(GameObject enemy)
        {
            if (enemy.Equals(this.gameObject))
            {
                _isDead = false;
                StartCoroutine(FireRoutine());
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                onDamagePlayer?.Invoke();
                StartCoroutine(DestroyEnemyRoutine());
            }
            else if (other.CompareTag("laser"))
            {
                onDestroyedEnemy?.Invoke(_pointValue);
                StartCoroutine(DestroyEnemyRoutine());
            }
            else if (other.CompareTag("Bounds"))
            {
                onResetEnemy?.Invoke(this.gameObject);
            }
        }

        IEnumerator DestroyEnemyRoutine()
        {
            _isDead = true;
            _collider.enabled = false;
            onSendSFXEnemy?.Invoke(2);
            var speed = _speed;
            _speed = 0;
            _anim.SetTrigger("Explode");
            _rend.enabled = false;
            yield return _delay;
            _rend.enabled = true;
            _speed = speed;
            _collider.enabled = true;
            onResetEnemy?.Invoke(this.gameObject);
        }

        IEnumerator FireRoutine()
        {
            while (_isDead == false)
            {
                float fireRate = Random.Range(1f, 3f);
                yield return new WaitForSeconds(fireRate);
                if (_isDead) yield break;
                PoolManager.Instance.EnemyGetLaser(_laserBarrel.position);
            }
        }

        private void OnDisable()
        {
            SpawnManager.onActivateEnemy -= ActivateEnemy;
        }
    }
}

