using Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _fireRate = .2f;
    [SerializeField] private int _lifeCount = 3;
    
    private float _nextFire = 0f;
    private bool _tripleShot;
    private WaitForSeconds _tripleShotDelay = new WaitForSeconds(5f);

    public static Action onPlayerDeath;


    private void OnEnable()
    {
        Enemy.onDamagePlayer += Damage;
        PowerUp.onCollectedPowerUp += TripleShotActive;
    }

    void Start()
    {
        transform.position = new Vector3(0f, -2f, 0f);
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
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

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
        _lifeCount--;

        if (_lifeCount < 1)
        {
            onPlayerDeath?.Invoke();
            Destroy(this.gameObject);
        }
    }

    private void TripleShotActive()
    {
        _tripleShot = true;
        StartCoroutine(TripleShotRoutine());
    }

    IEnumerator TripleShotRoutine()
    {
        yield return _tripleShotDelay;
        _tripleShot = false;
    }

    private void OnDisable()
    {
        Enemy.onDamagePlayer -= Damage;
        PowerUp.onCollectedPowerUp -= TripleShotActive;
    }
}
