using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YorkSDK.Util;

namespace Scripts.Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        private bool _gameOver;

        
        public override void Init()
        {
            base.Init();
        }

        private void OnEnable()
        {
            UIManager.onGameOver += GameOver;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        private void GameOver()
        {
            _gameOver = true;
            StartCoroutine(RestartRoutine());
        }

        IEnumerator RestartRoutine()
        {
            while (_gameOver == true)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    _gameOver = false;
                    SceneManager.LoadScene(1);  // Load current scene
                }

                yield return null;
            }
        }

        private void OnDisable()
        {
            UIManager.onGameOver -= GameOver;
        }
    }
}

