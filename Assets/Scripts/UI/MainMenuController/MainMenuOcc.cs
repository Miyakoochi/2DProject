using Audio;
using Core.QFrameWork;
using QFramework;
using UI.UICore;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuOcc : BaseController
    {
        public Button StartGame;
        public Button QuitGame;
        public Button CrateRoom;

        private void Awake()
        {
            StartGame.onClick.AddListener(OnStartGameClick);
            QuitGame.onClick.AddListener(OnQuitGameClick);
            CrateRoom.onClick.AddListener(OnCrateRoomClick);
        }

        private void OnCrateRoomClick()
        {
            this.GetSystem<IUISystem>().ShowTips("Tips_FunctionError");
            //this.GetSystem<IAudioSystem>().PlayAudioOnce(EMusicType.Click);
            //this.GetSystem<IUISystem>().SetUIShow(UIType.ChooseNetWorkMode, true);
        }

        private void OnQuitGameClick()
        {
            this.GetSystem<IAudioSystem>().PlayAudioOnce(EMusicType.Click);
            Application.Quit();
        
            // 在编辑器中停止播放模式
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

        private void OnStartGameClick()
        {
            this.GetSystem<IAudioSystem>().PlayAudioOnce(EMusicType.Click);
            this.GetSystem<IUISystem>().SetUIShow(UIType.ChooseLevel, true);
            //this.GetSystem<IUISystem>().SetUIShow(UIType.MainMenu, false);
        }
    }
}