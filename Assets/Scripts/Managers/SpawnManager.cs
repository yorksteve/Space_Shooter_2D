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
            StartCoroutine(SpawnRoutine());
        }

        private void StopSpawning()
        {
            _spawnEnemies = false;
        }

        IEnumerator SpawnRoutine()
        {
            while (_spawnEnemies == true)
            {
                var enemy = PoolManager.Instance.GetEnemy();
                float randomX = Random.Range(-9f, 9f);
                enemy.transform.position = new Vector3(randomX, _spawnHeight.position.y - _spawnOffset, 0);
                yield return new WaitForSeconds(Random.Range(1f, 5f));
            }
        }

        private void OnDisable()
        {
            Player.onPlayerDeath -= StopSpawning;
        }
    }
}

