using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YorkSDK.Util;

namespace Scripts.Menu
{
    public class MenuManager : MonoSingleton<MenuManager>
    {



        public override void Init()
        {
            base.Init();
        }

        public void StartGameButtonClicked()
        {
            SceneManager.LoadScene(1);
        }
    }
}

