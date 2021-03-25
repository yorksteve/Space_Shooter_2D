﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private int _powerupID;  // 0 = triple shot; 1 = speed; 2 = shields
    [SerializeField] private float _speed = 3f;

    public static Action<int> onCollectedPowerUp;
    public static Action<GameObject> onDeactivatePowerup;



    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onCollectedPowerUp?.Invoke(_powerupID);
            onDeactivatePowerup?.Invoke(this.gameObject);
        }
        else if (other.CompareTag("Bounds"))
        {
            onDeactivatePowerup?.Invoke(this.gameObject);
        }
    }
}
