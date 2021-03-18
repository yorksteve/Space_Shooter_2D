using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Managers
{
    public class PoolManager : MonoBehaviour
    {
        private static PoolManager _instance;
        public static PoolManager Instance
        {
            get
            {
                if (_instance == null)
                    Debug.LogError("PoolManager is NULL");

                return _instance;
            }
        }

        [SerializeField] private Transform _spawnPos;
        [SerializeField] private GameObject _laser;
        [SerializeField] private GameObject _laserContainer;
        [SerializeField] private List<GameObject> _laserPool;

        private int _laserQuantity = 10;
        private bool _initialCreation;



        private void Awake()
        {
            _instance = this;
        }

        private void OnEnable()
        {
            Laser.onDisableLaser += RecycleLaser;
        }

        void Start()
        {
            _initialCreation = true;
            GenerateLaserPool();
        }

        private GameObject CreateLaser()
        {
            GameObject laser = Instantiate(_laser, _spawnPos.position, Quaternion.identity, _laserContainer.transform);
            if (_initialCreation == true)
            {
                laser.SetActive(false);
            }
            _laserPool.Add(laser);

            return laser;
        }

        private List<GameObject> GenerateLaserPool()
        {
            for (int i = 0; i < _laserQuantity; i++)
            {
                CreateLaser();
            }

            return _laserPool;
        }

        public GameObject GetLaser()
        {
            foreach (var laser in _laserPool)
            {
                if (laser.activeInHierarchy == false)
                {
                    laser.transform.position = _spawnPos.position;
                    laser.SetActive(true);
                    return laser;
                }
            }

            _initialCreation = false;
            return CreateLaser();
        }

        private void RecycleLaser(GameObject laser)
        {
            laser.SetActive(false);
        }

        private void OnDisable()
        {
            Laser.onDisableLaser -= RecycleLaser;
        }
    }
}


