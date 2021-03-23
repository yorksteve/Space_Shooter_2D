using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Managers
{
    public class SpawnManager : MonoBehaviour
    {
        private static SpawnManager _instance;
        public static SpawnManager Instance
        {
            get
            {
                if (_instance == null)
                    Debug.LogError("SpawnManager is NULL");

                return _instance;
            }
        }

        [SerializeField] private Transform _spawnHeight;
        [SerializeField] private GameObject _tripleShotPowerUp;

        private float _spawnOffset = 1.0f;
        private bool _spawnEnemies = true;



        private void Awake()
        {
            _instance = this;
        }

        private void OnEnable()
        {
            Player.onPlayerDeath += StopSpawning;
        }

        private void Start()
        {
            StartCoroutine(SpawnEnemyRoutine());
            StartCoroutine(SpawnPowerUpRoutine());
        }

        private void StopSpawning()
        {
            _spawnEnemies = false;
        }

        IEnumerator SpawnEnemyRoutine()
        {
            while (_spawnEnemies == true)
            {
                var enemy = PoolManager.Instance.GetEnemy();
                float randomX = Random.Range(-9f, 9f);
                enemy.transform.position = new Vector3(randomX, _spawnHeight.position.y - _spawnOffset, 0);
                yield return new WaitForSeconds(Random.Range(1f, 5f));
            }
        }

        IEnumerator SpawnPowerUpRoutine()
        {
            while (_spawnEnemies == true)
            {
                var tripShot = Instantiate(_tripleShotPowerUp); // Change to pool later --after completion of other powerups
                float randomX = Random.Range(-9f, 9f);
                tripShot.transform.position = new Vector3(randomX, _spawnHeight.position.y - _spawnOffset, 0);
                yield return new WaitForSeconds(Random.Range(3f, 7f));
            }
        }

        private void OnDisable()
        {
            Player.onPlayerDeath -= StopSpawning;
        }
    }
}

