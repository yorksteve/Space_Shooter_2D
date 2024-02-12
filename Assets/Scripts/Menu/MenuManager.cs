using UnityEngine;
using UnityEngine.SceneManagement;
using YorkSDK.Util;

namespace Scripts.Menu
{
    public class MenuManager : MonoSingleton<MenuManager>
    {
        [SerializeField] private GameObject[] _menuObjects;


        public override void Init()
        {
            base.Init();
        }

        public void StartGameButtonClicked()
        {
            SceneManager.LoadScene(1);
        }

        public void ControlsButtonClicked()
        {
            foreach (var obj in _menuObjects)
            {
                obj.SetActive(!obj.activeInHierarchy);
            }
        }
    }
}

