using Audio;
using Core.QFrameWork;
using QFramework;
using SceneSystem;
using UI.UICore;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameStopMenuUI : BaseController
    {
        public Button ReturnGame;
        public Button ReturnMain;

        public void Awake()
        {
            ReturnGame.onClick.AddListener(OnReturnGame);
            ReturnMain.onClick.AddListener(OnReturnMain);
        }

        private void OnReturnMain()
        {
            Time.timeScale = 1.0f;
            this.GetSystem<IUISystem>().SetAllUIHide();
            this.GetSystem<IUISystem>().FadeIn();
            this.GetSystem<ISceneSystem>().ReturnMainMenu();
            this.GetSystem<IAudioSystem>().PlayBGM(EMusicType.MainMenu);
        }

        private void OnReturnGame()
        {
            Time.timeScale = 1.0f;
            this.GetSystem<IUISystem>().SetUIShow(UIType.GameStopMenu, false);
            this.GetSystem<IUISystem>().SetUIShow(UIType.StopGame, true);
        }
    }
}