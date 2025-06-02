using Core.QFrameWork;
using QFramework;
using UI.UICore;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BeginnerTutorial : BaseController
    {
        public Button Delete;

        private void Awake()
        {
            if (Delete)
            {
                Delete.onClick.AddListener(OnDeleteButton);
            }
        }

        private void OnDeleteButton()
        {
            this.GetSystem<IUISystem>().SetUIShow(UIType.BeginnerTutorial, false);
        }
    }
}