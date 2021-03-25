using Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Characters
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _speedBoost = 2;
        [SerializeField] private float _fireRate = .2f;
        [SerializeField] private int _lifeCount = 3;
        [SerializeField] private int _score;
        [SerializeField] private GameObject _shield;
        [SerializeField] private GameObject _rightEngine, _leftEngine;

        private float _nextFire = 0f;
        private bool _tripleShot;
        private bool _shieldActive;

        private WaitForSeconds _tripleShotDelay = new WaitForSeconds(5f);
        private WaitForSeconds _speedDelay = new WaitForSeconds(3f);

        public static Action onPlayerDeath;
        public static Action<int> onUpdateScore;
        public static Action<int> onUpdateLife;


        private void OnEnable()
        {
            Enemy.onDamagePlayer += Damage;
            PowerUp.onCollectedPowerUp += ActivatePowerup;
            Enemy.onDestroyedEnemy += Score;
        }

        void Start()
        {
            transform.position = new Vector3(0f, -2f, 0f);
            _shield.SetActive(false);
            _score = 0;
            onUpdateScore?.Invoke(_score);
            _rightEngine.SetActive(false);
            _leftEngine.SetActive(false);
        }

        void Update()
        {
            Movement();

            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
            {
                Fire();
            }
        }

        private void Movement()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(verticalInput, 0, -horizontalInput);

            transform.Translate(direction * _speed * Time.deltaTime);

            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.5f, 0f), transform.position.z);

            if (transform.position.x > 11.3f)
            {
                transform.position = new Vector3(-11.3f, transform.position.y, transform.position.z);
            }
            else if (transform.position.x < -11.3f)
            {
                transform.position = new Vector3(11.3f, transform.position.y, transform.position.z);
            }
        }

        private void Fire()
        {
            _nextFire = Time.time + _fireRate;
            if (_tripleShot == true)
            {
                PoolManager.Instance.GetTripShot();
            }
            else
            {
                PoolManager.Instance.GetLaser();
            }
        }

        private void Damage()
        {
            if (_shieldActive == true)
            {
                _shield.SetActive(false);
                _shieldActive = false;
                return;
            }

            _lifeCount--;
            onUpdateLife?.Invoke(_lifeCount);

            if (_lifeCount == 2)
            {
                _rightEngine.SetActive(true);
            }
            if (_lifeCount == 1)
            {
                _leftEngine.SetActive(true);
            }

            if (_lifeCount < 1)
            {
                onPlayerDeath?.Invoke();
                Destroy(this.gameObject);
            }
        }

        private void ActivatePowerup(int id)
        {
            switch (id)
            {
                case 0:
                    _tripleShot = true;
                    StartCoroutine(TripleShotRoutine());
                    break;
                case 1:
                    StartCoroutine(SpeedBoostRoutine());
                    break;
                case 2:
                    _shieldActive = true;
                    _shield.SetActive(true);
                    break;
                default:
                    Debug.Log("Non-Applicable value");
                    break;
            }
        }

        private void Score(int points)
        {
            _score += points;
            onUpdateScore?.Invoke(_score);
        }

        IEnumerator TripleShotRoutine()
        {
            yield return _tripleShotDelay;
            _tripleShot = false;
        }

        IEnumerator SpeedBoostRoutine()
        {
            _speed *= _speedBoost;
            yield return _speedDelay;
            _speed /= _speedBoost;
        }

        private void OnDisable()
        {
            Enemy.onDamagePlayer -= Damage;
            PowerUp.onCollectedPowerUp -= ActivatePowerup;
            Enemy.onDestroyedEnemy -= Score;
        }
    }
}

