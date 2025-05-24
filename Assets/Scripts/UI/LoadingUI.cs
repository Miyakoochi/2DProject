using AssetSystem;
using Core.QFrameWork;
using LevelSystem;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LoadingUI : BaseController
    {
        public Slider ProcessBar;

        private IAddressableModel mAddressableModel;
        
        private float mMinLoadTime = 1f;
        private float mLoadTimeElapsed = 0.0f;
        
        private void Awake()
        {
            mAddressableModel = this.GetModel<IAddressableModel>();
            //this.RegisterEvent<LoadSceneAssetStartEvent>(OnLoadSceneAssetStart);
            //this.RegisterEvent<LoadGameSceneCompleteEvent>(OnLoadGameSceneComplete);

            this.RegisterEvent<StartCheckAndUpdateAssetEvent>(OnStartCheckAndUpdateAsset).UnRegisterWhenDisabled(this);
            this.RegisterEvent<ErrorCheckAndUpdateAssetEvent>(OnErrorCheckAndUpdateAsset).UnRegisterWhenDisabled(this);
        }

        private void OnErrorCheckAndUpdateAsset(ErrorCheckAndUpdateAssetEvent obj)
        {
            //显示消息窗口
            Debug.Log("OnErrorCheckAndUpdateAsset" + obj.Message);
        }

        private void OnStartCheckAndUpdateAsset(StartCheckAndUpdateAssetEvent obj)
        {
            Debug.Log("OnStartCheckAndUpdateAsset");
        }

        private void Update()
        {
            var ratio = mLoadTimeElapsed / mMinLoadTime;
            mLoadTimeElapsed += Time.deltaTime;
            ProcessBar.value = Mathf.Max(ratio, mAddressableModel.ShowProcessValue.Value);
        }
    }
}