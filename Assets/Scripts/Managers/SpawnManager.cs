﻿using Scripts.Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;
using Random = UnityEngine.Random;

namespace Scripts.Managers
{
    public class SpawnManager : MonoSingleton<SpawnManager>
    {
        [SerializeField] private Transform _spawnHeight;

        private float _spawnOffset = 1.5f;
        private bool _spawnEnemies = true;
        private int _powerupNumber;
        private WaitForSeconds _spawnDelay = new WaitForSeconds(3f);

        public static Action<GameObject> onActivateEnemy;



        public override void Init()
        {
            base.Init();
        }

        private void OnEnable()
        {
            Player.onPlayerDeath += StopSpawning;
            Astroid.onAstroidExplosion += StartSpawning;
        }

        private void Start()
        {
            _powerupNumber = PoolManager.Instance.PowerupNumber();
        }

        private void StartSpawning()
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
            yield return _spawnDelay;

            while (_spawnEnemies == true)
            {
                var enemy = PoolManager.Instance.GetEnemy();
                float randomX = Random.Range(-9f, 9f);
                enemy.transform.position = new Vector3(randomX, _spawnHeight.position.y - _spawnOffset, 0);
                onActivateEnemy?.Invoke(enemy);
                yield return new WaitForSeconds(Random.Range(1f, 5f));
            }
        }

        IEnumerator SpawnPowerUpRoutine()
        {
            yield return _spawnDelay;

            while (_spawnEnemies == true)
            {
                int randomID = Random.Range(0, _powerupNumber);
                float randomX = Random.Range(-9f, 9f);
                var powerup = PoolManager.Instance.GetPowerup(randomID);
                powerup.transform.position = new Vector3(randomX, _spawnHeight.position.y - _spawnOffset, 0);
                if (randomID == 3)
                {
                    powerup.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
                }
                if (randomID == 4)
                {
                    powerup.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                }
                yield return new WaitForSeconds(Random.Range(3f, 7f));
            }
        }

        private void OnDisable()
        {
            Player.onPlayerDeath -= StopSpawning;
            Astroid.onAstroidExplosion += StartSpawning;
        }
    }
}

