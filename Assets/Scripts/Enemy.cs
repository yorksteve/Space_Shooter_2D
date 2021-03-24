using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    [SerializeField] private int _pointValue = 10;

    public static Action onDamagePlayer;
    public static Action<GameObject> onResetEnemy;
    public static Action<int> onDestroyedEnemy;


    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            onDamagePlayer?.Invoke();
        }
        else if (other.CompareTag("laser"))
        {
            onDestroyedEnemy?.Invoke(_pointValue);
        }

        onResetEnemy?.Invoke(this.gameObject);
    }
}
