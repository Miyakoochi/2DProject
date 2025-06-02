using Core.QFrameWork;
using QFramework;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.UICore
{
    public class UIManager : BaseController
    {
        public Transform UICanvas;
        public Transform DamageCanvas;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            this.RegisterEvent<InitUIEvent>(OnInitUIEvent).UnRegisterWhenDisabled(this);
        }

        private void OnInitUIEvent(InitUIEvent @event)
        {
            this.GetModel<IUIModel>().StaticUIs = UICanvas;
            this.GetModel<IUIModel>().DamageUIs = DamageCanvas;
            
            this.GetSystem<IUISystem>().LoadLocalAllUIAsset();
        }
    }
}