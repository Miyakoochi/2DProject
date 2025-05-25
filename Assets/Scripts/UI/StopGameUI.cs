using Core.QFrameWork;
using QFramework;
using UI.UICore;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StopGameUI : BaseController
    {
        public Button StopButton;

        private void Awake()
        {
            if (StopButton)
            {
                StopButton.onClick.AddListener(OnStopButton);
            }
        }

        private void OnStopButton()
        {
            Time.timeScale = 0.0f;
            this.GetSystem<IUISystem>().SetUIShow(UIType.GameStopMenu, true);
            this.GetSystem<IUISystem>().SetUIShow(UIType.StopGame, false);
        }
    }
}