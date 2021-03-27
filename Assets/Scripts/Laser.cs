using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 8;
    [SerializeField] private bool _isEnemy;

    public static Action<GameObject> onDisableLaser;
    public static Action onPlayerHit;



    private void Update()
    {
        if (_isEnemy == true)
        {
            EnemyLaser();
        }
        else
        {
            PlayerLaser();
        }
    }

    private void EnemyLaser()
    {
        transform.Translate(Vector3.right * _speed * .75f * Time.deltaTime);
    }

    private void PlayerLaser()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isEnemy == true)
        {
            if (other.CompareTag("Player"))
            {
                onPlayerHit?.Invoke();
                onDisableLaser?.Invoke(this.gameObject);
            }
            else if (!other.CompareTag("Enemy"))
            {
                onDisableLaser?.Invoke(this.gameObject);
            }
        }
        else
        {
            if (other.CompareTag("Bounds") && this.transform.parent.gameObject.CompareTag("TripleShot"))
            {
                onDisableLaser?.Invoke(this.transform.parent.gameObject);
            }
            else
            {
                onDisableLaser?.Invoke(this.gameObject);
            }
        }
    }
}
