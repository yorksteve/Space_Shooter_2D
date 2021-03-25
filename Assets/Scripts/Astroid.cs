using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField] private float _speed = 55;
    
    private Animator _anim;
    private SpriteRenderer _rend;
    private WaitForSeconds _delay = new WaitForSeconds(1f);

    public static Action onAstroidExplosion;


    private void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        _rend = GetComponent<SpriteRenderer>();

        if (_anim == null)
            Debug.LogError("Astroid animator is NULL");

        if (_rend == null)
            Debug.LogError("Astroid renderer is NULL");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(DestroyRoutine());
    }

    IEnumerator DestroyRoutine()
    {
        _anim.SetTrigger("Explode");
        _rend.enabled = false;
        yield return _delay;
        onAstroidExplosion?.Invoke();
        Destroy(this.gameObject);
    }
}
