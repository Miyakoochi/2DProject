using System.Collections.Generic;
using Core.QFrameWork;
using UnityEngine;

namespace UI.UICore
{
    /// <summary>
    /// 单个UI页面的控制组件 相当于MVC的Controller
    /// 其中注入子对象的组件 进行操作 从而实现从细粒度到更粗粒度的UI控制
    /// </summary>
    public class UIControllerComponent : BaseController
    {
        [SerializeField]
        protected CanvasGroup mCanvasGroup;

        [SerializeField]
        protected List<UITweener> mUITweener;

        [SerializeField] 
        protected List<UIControllerComponent> mChildrenUIController;
        
        public bool UseTweenToShow = false;
        public bool UseTweenToHidden = false;
        private void Awake()
        {
            if (!mCanvasGroup)
            {
                mCanvasGroup = GetComponent<CanvasGroup>();
            }
        }

        /// <summary>
        /// 设置UI以什么样的方式进行展示
        /// 如果设置了Tween则会按照Tween的方式执行
        /// 否则直接显示
        /// </summary>
        public virtual void SetUIShow()
        {
            if (UseTweenToShow == false && mCanvasGroup)
            {
                mCanvasGroup.alpha = 1;
                SetCanInteractable(true);
            }
            else
            {
                foreach (var tweener in mUITweener)
                {
                    tweener?.EnterTween(CanInteractable);
                }
            }

            foreach (var uiControllerComponent in mChildrenUIController)
            {
                uiControllerComponent.SetUIShow();
            }
        }

        /// <summary>
        /// 设置UI以什么样的方式进行隐藏
        /// 如果设置了Tween则会按照Tween的方式执行
        /// 否则直接隐藏
        /// </summary>
        public virtual void SetUIHidden()
        {
            if (UseTweenToHidden == false && mCanvasGroup)
            {
                mCanvasGroup.alpha = 0;
                SetCanInteractable(false);
            }
            else
            {
                foreach (var tweener in mUITweener)
                {
                    tweener?.ExitTween(CannotInteractable);
                    //SetCanInteractable(false);
                }
            }

            foreach (var uiControllerComponent in mChildrenUIController)
            {
                uiControllerComponent.SetUIHidden();
            }
        }

        protected void SetCanInteractable(bool doCan)
        {
            if (!mCanvasGroup) return;
            
            mCanvasGroup.interactable = doCan;
            mCanvasGroup.blocksRaycasts = doCan;
        }

        private void CanInteractable()
        {
            SetCanInteractable(true);
        }
        
        private void CannotInteractable()
        {
            SetCanInteractable(false);
        }
    }
}