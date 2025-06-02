using Audio;
using Core.QFrameWork;
using QFramework;
using SceneSystem;
using UI.UICore;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameEndMenu
{
    public class GameEndMenuUI : BaseController
    {
        public Button ReturnMain;

        private void Awake()
        {
            if (ReturnMain)
            {
                ReturnMain.onClick.AddListener(OnReturnMain);
            }
        }

        private void OnReturnMain()
        {
            Time.timeScale = 1.0f;
            this.GetSystem<IUISystem>().SetAllUIHide();
            this.GetSystem<IUISystem>().FadeIn();
            this.GetSystem<ISceneSystem>().ReturnMainMenu();
            this.GetSystem<IAudioSystem>().PlayBGM(EMusicType.MainMenu);
        }
    }
}