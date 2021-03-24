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



        public override void Init()
        {
            base.Init();
        }

        private void OnEnable()
        {
            Player.onUpdateScore += UpdateScore;
        }

        private void UpdateScore(int score)
        {
            _scoreTxt.text = "Score: " + score.ToString();
        }

        private void OnDisable()
        {
            Player.onUpdateScore -= UpdateScore;
        }
    }
}

