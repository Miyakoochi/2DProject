using Core.QFrameWork;
using NetWorkSystem;
using QFramework;
using UI.UICore;
using UnityEngine.UI;

namespace UI
{
    public class WatingController : BaseController
    {
        public Button Delete;

        private void Awake()
        {
            if (Delete)
            {
                Delete.onClick.AddListener(OnDeleteClick);
            }
        }

        private void OnDeleteClick()
        {
            this.GetSystem<INetWorkSystem>().CloseNetWork();
            this.GetSystem<IUISystem>().SetUIShow(UIType.Wating, false);
        }
    }
}