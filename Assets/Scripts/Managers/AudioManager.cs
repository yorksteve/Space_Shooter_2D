using Scripts.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;


namespace Scripts.Managers
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        [SerializeField] private AudioClip[] _sfxClips;
        [SerializeField] private AudioSource _sfxSource;


        public override void Init()
        {
            base.Init();
        }

        private void OnEnable()
        {
            Player.onSendSFX += PlaySFX;
            Enemy.onSendSFXEnemy += PlaySFX;
            Astroid.onExplodeAstroidSFX += PlaySFX;
        }

        private void PlaySFX(int id)
        {
            _sfxSource.PlayOneShot(_sfxClips[id]);
        }

        private void OnDisable()
        {
            Player.onSendSFX -= PlaySFX;
            Enemy.onSendSFXEnemy -= PlaySFX;
            Astroid.onExplodeAstroidSFX -= PlaySFX;
        }
    }
}

