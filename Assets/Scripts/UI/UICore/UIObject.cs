using Core;
using ObjectPool;
using UnityEngine;

namespace UI.UICore
{
    public class UIObject : IGameObject, IPoolable<UIDataModel>
    {
        private UIControllerComponent mComponent;

        public UIObject(UIDataModel dataModel)
        {
            if (dataModel.UIPrefabs)
            {
                Self = GameObject.Instantiate(dataModel.UIPrefabs);
                mComponent = Self.GetComponent<UIControllerComponent>();
            }
        }

        protected UIObject()
        {
            
        }

        public void SetShow(bool isActive)
        {
            if (!mComponent) return;

            if (isActive)
            {
                mComponent.SetUIShow();
                return;
            }
            
            mComponent.SetUIHidden();
        }


        public GameObject Self { get; set; }
        public void Set(UIDataModel dataModel)
        {
            if (dataModel.UIPrefabs)
            {
                Self = GameObject.Instantiate(dataModel.UIPrefabs);
                mComponent = Self.GetComponent<UIControllerComponent>();
                Self.SetActive(true);
            }
        }

        public void Reset()
        {
            Self.SetActive(false);
        }
    }
}