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

        [SerializeField] private Transform _laserSpawnPos;
        [SerializeField] private GameObject _laser;
        [SerializeField] private GameObject _laserContainer;
        [SerializeField] private List<GameObject> _laserPool;

        [SerializeField] private GameObject _tripleShotLaser;
        [SerializeField] private GameObject _tripleShotContainer;
        [SerializeField] private List<GameObject> _tripleShotPool;

        [SerializeField] private GameObject _enemy;
        [SerializeField] private GameObject _enemyContainer;
        [SerializeField] private List<GameObject> _enemyPool;

        private int _laserQuantity = 10;
        private int _enemyQuantity = 10;
        private bool _initialCreation;



        private void Awake()
        {
            _instance = this;
        }

        private void OnEnable()
        {
            Laser.onDisableLaser += RecycleObject;
            Enemy.onResetEnemy += RecycleObject;
        }

        void Start()
        {
            _initialCreation = true;
            GenerateLaserPool();
            GenerateTripShotPool();
            GenerateEnemyPool();
        }

        private GameObject CreateLaser()
        {
            GameObject laser = Instantiate(_laser, _laserSpawnPos.position, Quaternion.identity, _laserContainer.transform);
            if (_initialCreation == true)
            {
                laser.SetActive(false);
            }
            _laserPool.Add(laser);

            return laser;
        }

        private GameObject CreateTripleShot()
        {
            GameObject tripShot = Instantiate(_tripleShotLaser, _laserSpawnPos.position, Quaternion.identity, _tripleShotContainer.transform);
            if (_initialCreation == true)
            {
                tripShot.SetActive(false);
            }
            _tripleShotPool.Add(tripShot);

            return tripShot;
        }

        private GameObject CreateEnemy()
        {
            GameObject enemy = Instantiate(_enemy, _enemyContainer.transform.position, Quaternion.identity, _enemyContainer.transform);
            enemy.SetActive(false);
            _enemyPool.Add(enemy);

            return enemy;
        }

        private List<GameObject> GenerateLaserPool()
        {
            for (int i = 0; i < _laserQuantity; i++)
            {
                CreateLaser();
            }

            return _laserPool;
        }

        private List<GameObject> GenerateTripShotPool()
        {
            for (int i = 0; i < _laserQuantity; i++)
            {
                CreateTripleShot();
            }

            return _tripleShotPool;
        }

        private List<GameObject> GenerateEnemyPool()
        {
            for (int i = 0; i < _enemyQuantity; i++)
            {
                CreateEnemy();
            }

            return _enemyPool;
        }

        public GameObject GetLaser()
        {
            foreach (var laser in _laserPool)
            {
                if (laser.activeInHierarchy == false)
                {
                    laser.transform.position = _laserSpawnPos.position;
                    laser.SetActive(true);
                    return laser;
                }
            }

            _initialCreation = false;
            return CreateLaser();
        }

        public GameObject GetTripShot()
        {
            foreach (var shot in _tripleShotPool)
            {
                if (shot.activeInHierarchy == false)
                {
                    shot.transform.position = _laserSpawnPos.position;
                    shot.SetActive(true);
                    return shot;
                }
            }

            _initialCreation = false;
            return CreateTripleShot();
        }

        public GameObject GetEnemy()
        {
            foreach (var enemy in _enemyPool)
            {
                if (enemy.activeInHierarchy == false)
                {
                    enemy.SetActive(true);
                    return enemy;
                }
            }

            return CreateEnemy();
        }

        private void RecycleObject(GameObject obj)
        {
            obj.SetActive(false);
        }

        private void OnDisable()
        {
            Laser.onDisableLaser -= RecycleObject;
            Enemy.onResetEnemy -= RecycleObject;
        }
    }
}


