using Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _fireRate = .2f;
    
    private float _fireDelay = 0f;



    void Start()
    {
        transform.position = new Vector3(0f, -2f, 0f);
    }

    void Update()
    {
        Movement();
        
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _fireDelay)
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
        _fireDelay = Time.time + _fireRate;
        PoolManager.Instance.GetLaser();
    }
}
