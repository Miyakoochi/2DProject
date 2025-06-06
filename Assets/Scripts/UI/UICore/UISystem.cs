﻿using System.Collections.Generic;
using AssetSystem;
using Common;
using Cysharp.Threading.Tasks;
using QFramework;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UI.UICore
{
    
    public interface IUISystem : ISystem
    {
        /// <summary>
        /// 根据类型显示或隐藏UI
        /// 如果没找到已经创建的UI则创建一个
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isShow"></param>
        public void SetUIShow(UIType type, bool isShow);

        public void SetAllUIHide();
        
        /// <summary>
        /// 加载本地所有的UI资源
        /// 在更新并获取远程资源之前调用
        /// 用于处理加载界面的UI
        /// 或者直接加载所有的UI（远程没有UI资源）
        /// </summary>
        public UniTask<AsyncOperationHandle<IList<UIDataModel>>> LoadLocalAllUIAsset();

        /// <summary>
        /// 展示Tips
        /// </summary>
        /// <param name="tipsKey"></param>
        public void ShowTips(string tipsKey);

        public void FadeOut();
        public void FadeIn();
    }
    
    public class UISystem : AbstractSystem, IUISystem
    {
        private struct UIPageNode
        {
            
        }
        
        /// <summary>
        /// 用于查找UI对象
        /// </summary>
        private Dictionary<UIType, UIObject> mUIObjects = new();
        private Dictionary<UIType, UIDataModel> mDataModels = new();
        private AsyncOperationHandle<IList<UIDataModel>> mOperationHandles = new();

        //private Stack<>
        
        private IUIModel mUIModel;

        public async UniTask<AsyncOperationHandle<IList<UIDataModel>>> LoadLocalAllUIAsset()
        {
            if (mOperationHandles.IsValid() && mOperationHandles.Status == AsyncOperationStatus.Succeeded) return mOperationHandles;
            
            mOperationHandles = await this.GetSystem<IAddressableSystem>().LoadAssetsAsync<UIDataModel>(new List<string>(){ Util.UIDataModelTag }, null);
            if (mOperationHandles.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var dataModel in mOperationHandles.Result)
                {
                    mDataModels.TryAdd(dataModel.type, dataModel);
                }
            }
            
            this.SendEvent<EndInitUIEvent>();
            return mOperationHandles;
        }
        
        public void SetUIShow(UIType type, bool isShow)
        {
            if (mDataModels.Count <= 0)
            {
                //await LoadLocalAllUIAsset();
                return;
            }
            
            if(mOperationHandles.Status != AsyncOperationStatus.Succeeded) return;
            
            if (mUIObjects.TryGetValue(type, out var value))
            {
                value.SetShow(isShow);
            }
            else
            {
                if(mDataModels.TryGetValue(type, out var model) == false) return;
                
                var uiObject = new UIObject(model);
                mUIObjects.TryAdd(model.type, uiObject);
                uiObject.Self?.transform.SetParent(mUIModel.StaticUIs ,false);
                uiObject.SetShow(isShow);
            }
            
        }

        public void FadeOut()
        {
            this.SendEvent<TransitionEndEvent>();
        }

        public void FadeIn()
        {
            this.SendEvent<TransitionStartEvent>();
        }

        public void SetAllUIHide()
        {
            foreach (var kvp in mUIObjects)
            {
                kvp.Value.SetShow(false);
            }
        }
        
        public void ShowTips(string tipsKey)
        {
            var loadingResult = LocalizationSettings.StringDatabase.GetTableEntry("TestTable", tipsKey);
            this.SendEvent(new ShowTipsEvent()
            {
                Tips = loadingResult.Entry.GetLocalizedString()
            });

            SetUIShow(UIType.Tips, true);
        }
        
        protected override void OnInit()
        {
            mUIModel = this.GetModel<IUIModel>();
        }

        protected override void OnDeinit()
        {
            base.OnDeinit();
            
            mUIObjects.Clear();
            mDataModels.Clear();
            mOperationHandles.Release();
        }
    }
}