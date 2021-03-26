using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Characters
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float _speed = 4f;
        [SerializeField] private int _pointValue = 10;
        
        private Animator _anim;
        private Renderer _rend;
        private WaitForSeconds _delay = new WaitForSeconds(2.8f);

        public static Action onDamagePlayer;
        public static Action<GameObject> onResetEnemy;
        public static Action<int> onDestroyedEnemy;
        public static Action<int> onSendSFXEnemy;


        private void Start()
        {
            _anim = GetComponentInChildren<Animator>();
            _rend = GetComponentInChildren<Renderer>();

            if (_anim == null)
                Debug.LogError("Animator is Null");

            if (_rend == null)
                Debug.LogError("Renderer is NULL");
        }

        void Update()
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
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
            onSendSFXEnemy?.Invoke(2);
            var speed = _speed;
            _speed = 0;
            _anim.SetTrigger("Explode");
            _rend.enabled = false;
            yield return _delay;
            _rend.enabled = true;
            _speed = speed;
            onResetEnemy?.Invoke(this.gameObject);
        }
    }
}

