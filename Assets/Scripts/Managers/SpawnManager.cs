using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;

namespace Scripts.Managers
{
    public class SpawnManager : MonoSingleton<SpawnManager>
    {
        [SerializeField] private Transform _spawnHeight;

        private float _spawnOffset = 1.5f;
        private bool _spawnEnemies = true;



        public override void Init()
        {
            base.Init();
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
                int randomID = Random.Range(0, 3);
                float randomX = Random.Range(-9f, 9f);
                var powerup = PoolManager.Instance.GetPowerup(randomID);
                powerup.transform.position = new Vector3(randomX, _spawnHeight.position.y - _spawnOffset, 0);
                yield return new WaitForSeconds(Random.Range(3f, 7f));
            }
        }

        private void OnDisable()
        {
            Player.onPlayerDeath -= StopSpawning;
        }
    }
}

