using Scripts.Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YorkSDK.Util;

namespace Scripts.Managers
{
    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField] private Text _scoreTxt;
        [SerializeField] private Image _lifeImg;
        [SerializeField] private Sprite[] _lifeCount;
        [SerializeField] private Text _gameOverTxt;
        [SerializeField] private Text _restartTxt;

        private WaitForSeconds _flickerDelay = new WaitForSeconds(.5f);

        public static Action onGameOver;



        public override void Init()
        {
            base.Init();
        }

        private void OnEnable()
        {
            Player.onUpdateScore += UpdateScore;
            Player.onUpdateLife += UpdateLives;
        }

        private void Start()
        {
            UpdateLives(3);
            _gameOverTxt.gameObject.SetActive(false);
            _restartTxt.gameObject.SetActive(false);
        }

        private void UpdateScore(int score)
        {
            _scoreTxt.text = "Score: " + score.ToString();
        }

        private void UpdateLives(int currentLife)
        {
            _lifeImg.sprite = _lifeCount[currentLife];
            if (currentLife == 0)
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            onGameOver?.Invoke();
            _gameOverTxt.gameObject.SetActive(true);
            _restartTxt.gameObject.SetActive(true);
            StartCoroutine(GameOverFlickerRoutine());
        }

        IEnumerator GameOverFlickerRoutine()
        {
            while (true)
            {
                _gameOverTxt.gameObject.SetActive(false);
                yield return _flickerDelay;
                _gameOverTxt.gameObject.SetActive(true);
                yield return _flickerDelay;
            }
        }

        private void OnDisable()
        {
            Player.onUpdateScore -= UpdateScore;
            Player.onUpdateLife -= UpdateLives;
        }
    }
}

