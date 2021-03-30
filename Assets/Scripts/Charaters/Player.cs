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
        [SerializeField] private Transform _rightBarrel, _leftBarrel;

        private float _nextFire = 0f;
        private bool _tripleShot;
        private bool _shieldActive;
        private bool _rightBarrelActive;

        private SpriteRenderer _rend;
        private Color _initialColor;
        private int _shieldStrength = 3;
        private int _ammoCount = 15;

        private WaitForSeconds _tripleShotDelay = new WaitForSeconds(5f);
        private WaitForSeconds _speedDelay = new WaitForSeconds(3f);

        public static Action onPlayerDeath;
        public static Action onHit;
        public static Action<int> onUpdateScore;
        public static Action<int> onUpdateLife;
        public static Action<int> onSendSFX;
        public static Action<int> onUpdateAmmo;


        private void OnEnable()
        {
            Enemy.onDamagePlayer += Damage;
            PowerUp.onCollectedPowerUp += ActivatePowerup;
            Enemy.onDestroyedEnemy += Score;
            Laser.onPlayerHit += Damage;
        }

        void Start()
        {
            transform.position = new Vector3(0f, -2f, 0f);
            _rend = _shield.GetComponent<SpriteRenderer>();
            if (_rend != null)
            {
                _initialColor = _rend.material.color;
            }
            _shield.SetActive(false);
            _score = 0;
            onUpdateScore?.Invoke(_score);
            _rightEngine.SetActive(false);
            _leftEngine.SetActive(false);
            RestoreAmmo();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _speed *= 1.5f;
            }
            
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _speed /= 1.5f;
            }

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
            if (_ammoCount != 0)
            {
                _ammoCount--;
                onUpdateAmmo?.Invoke(_ammoCount);
                _nextFire = Time.time + _fireRate;
                if (_tripleShot == true)
                {
                    PoolManager.Instance.GetTripShot();
                }
                else
                {
                    if (_rightBarrelActive == true)
                    {
                        PoolManager.Instance.GetLaser(_rightBarrel.position);
                        _rightBarrelActive = false;
                    }
                    else
                    {
                        PoolManager.Instance.GetLaser(_leftBarrel.position);
                        _rightBarrelActive = true;
                    }
                }

                onSendSFX?.Invoke(0);
            }
        }

        private void Damage()
        {
            if (_shieldActive == true)
            {
                _shieldStrength--;
                ShieldControl();
                return;
            }

            onHit?.Invoke();
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
                onSendSFX?.Invoke(2);
                onPlayerDeath?.Invoke();
                Destroy(this.gameObject);
            }
        }

        private void Heal()
        {
            if (_lifeCount < 3)
            {
                _lifeCount++;
                onUpdateLife?.Invoke(_lifeCount);
            }
            if (_lifeCount == 3)
            {
                _rightEngine.SetActive(false);
            }
            if (_lifeCount == 2)
            {
                _leftEngine.SetActive(false);
            }
        }

        private void ActivatePowerup(int id)
        {
            onSendSFX?.Invoke(1);

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
                    if (_shieldActive == false)
                    {
                        _shieldStrength = 3;
                        _shieldActive = true;
                        _shield.SetActive(true);
                        ShieldControl();
                    }
                    break;
                case 3:
                    Heal();
                    break;
                case 4:
                    RestoreAmmo();
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

        private void ShieldControl()
        {
            switch (_shieldStrength)
            {
                case 3:
                    _rend.material.color = _initialColor;
                    break;
                case 2:
                    _rend.material.color = Color.yellow;
                    break;
                case 1:
                    _rend.material.color = Color.red;
                    break;
                case 0:
                    DeactivateShield();
                    break;
            }
        }

        private void DeactivateShield()
        {
            _shieldActive = false;
            _shield.SetActive(false);
        }

        private void RestoreAmmo()
        {
            _ammoCount = 15;
            onUpdateAmmo?.Invoke(_ammoCount);
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
            Laser.onPlayerHit -= Damage;
        }
    }
}

