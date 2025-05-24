using Core.QFrameWork;
using QFramework;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.UICore
{
    public class UIController : BaseController
    {
        public Transform UICanvas;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            this.RegisterEvent<InitUIEvent>(OnInitUIEvent).UnRegisterWhenDisabled(this);
        }

        private void OnInitUIEvent(InitUIEvent @event)
        {
            //if(!StaticUIs) Debug.Log("StaticUIs is null");
            //if(!DynamicUIs) Debug.Log("DynamicUIs is null");
            //if(!OccasionallyUIs) Debug.Log("OccasionallyUIs is null");
            
            this.GetModel<IUIModel>().StaticUIs = UICanvas;
            
            this.GetSystem<IUISystem>().LoadLocalAllUIAsset();
        }
    }
}