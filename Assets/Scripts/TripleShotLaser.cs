using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShotLaser : MonoBehaviour
{
    public static Action<GameObject> onResetTripShot;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bounds"))
        {
            onResetTripShot?.Invoke(this.gameObject);
        }
    }
}
