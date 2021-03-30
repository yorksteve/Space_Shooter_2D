using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterBoost : MonoBehaviour
{
    [SerializeField] private GameObject _currentObj;
    [SerializeField] private float _maxScale;// = 10f;
    [SerializeField] private float _maxFuel;
    private float _currentFuel;
    private Material _material;
    private Renderer _rend;
    private float _fuelPercent;
    private bool _thrusterBoost;


    private void Awake()
    {
        _rend = GetComponent<Renderer>();
        _material = _rend.materials[0];
        _fuelPercent = 1f;
        gameObject.transform.localScale = new Vector3(_maxScale, .2f, .05f);
    }

    private void OnEnable()
    {
        
    }

    private void StartBoost()
    {
        _thrusterBoost = true;
        StartCoroutine(ModifyFuelRoutine());
    }

    private void EndBoost()
    {
        _thrusterBoost = false;
    }

    IEnumerator ModifyFuelRoutine()
    {
        while (_thrusterBoost == true)
        {
            float fuel = 1f;
            _currentFuel = fuel;
            _fuelPercent = _currentFuel / _maxFuel;
            _material.SetFloat("_fuelPercent", _fuelPercent);
            gameObject.transform.localScale = new Vector3(Mathf.Clamp(_fuelPercent * _maxScale, 0, _maxScale), .5f, .05f);
            yield return null;
        }
    }

    private void OnDisable()
    {
        
    }
}
