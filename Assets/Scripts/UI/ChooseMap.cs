using Audio;
using Core.QFrameWork;
using QFramework;
using SceneSystem;
using UI.UICore;
using UnityEngine.UI;

namespace UI
{
    public class ChooseMap : BaseController
    {
        public Button Map1Button;

        private void Start()
        {
            if (Map1Button)
            {
                Map1Button.onClick.AddListener(OnMap1Button);
            }
        }

        private void OnMap1Button()
        {
            this.GetSystem<IAudioSystem>().PlayAudioOnce(EMusicType.Click);
            this.GetSystem<ISceneSystem>().LoadGameScene(1002, 0);
            this.GetSystem<IUISystem>().SetAllUIHide();
            this.GetSystem<IUISystem>().FadeIn();
        }
    }
}