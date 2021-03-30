using Scripts.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 _initialPos;
    private float _shakeFrequency = .7f;


    private void OnEnable()
    {
        Player.onHit += ShakeCamera;
    }

    private void Start()
    {
        _initialPos = transform.position;
    }

    private void ShakeCamera()
    {
        StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        int i = 5;
        while (i > 0)
        {
            i--;
            transform.position += Random.insideUnitSphere * _shakeFrequency;
            yield return null;
        }

        transform.position = _initialPos;
    }

    private void OnDisable()
    {
        Player.onHit -= ShakeCamera;
    }
}
