using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 8;

    public static Action<GameObject> onDisableLaser;



    private void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
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
