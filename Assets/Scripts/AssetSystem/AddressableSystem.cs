using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using QFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AssetSystem
{
    /// <summary>
    /// 资源加载使用RemotePath路径处进行加载，
    /// </summary>
    public interface IAddressableSystem : ISystem
    {
        /// <summary>
        /// 加载的时候需要指定加载的资源的类型
        /// 因此资源的分类应该按照类型Label进行分类
        /// </summary>
        /// <param name="address"></param>
        /// <param name="completed"></param>
        /// <typeparam name="T"></typeparam>

        public UniTask<AsyncOperationHandle<T>> LoadAssetAsync<T>(string addressPath);
        public UniTask<AsyncOperationHandle<T>> LoadAssetAsync<T>(AssetReference reference);
        public UniTask<AsyncOperationHandle<IList<T>>> LoadAssetsAsync<T>(List<string> tags, Action<T> completed);

        /// <summary>
        /// 检查并更新远程资产
        /// 将进度、文件大小等信息同步到Model中
        /// 部分本地资产是不需要远程获取的
        /// </summary>
        public void CheckAndDownloadRemoteAsset();
    }
    
    public class AddressableSystem : AbstractSystem, IAddressableSystem
    {
        private Dictionary<string, AsyncOperationHandle> mCache = new(StringComparer.Ordinal);  
        private Dictionary<AssetReference, AsyncOperationHandle> mReferenceCache = new();

        private IAddressableModel mAddressableModel;

        public void LoadAsset<T>(string address, Action<T> completed)
        {
            if (mCache.TryGetValue(address, out var value))
            {
                completed?.Invoke((T)value.Result);
                return;
            }
            
            var handle = Addressables.LoadAssetAsync<T>(address);
            handle.Completed += operationHandle =>
            {
                completed?.Invoke(operationHandle.Result);
                mCache[address] = operationHandle;
            } ;
        }

        public void LoadAsset<T>(AssetReference reference, Action<T> completed)
        {
        }

        public void LoadAsset<T>(AssetLabelReference reference, Action<T> completed)
        {
        }

        public async UniTask<AsyncOperationHandle<T>> LoadAssetAsync<T>(string addressPath)
        {
            var address = Addressables.LoadAssetAsync<T>(addressPath);
            await address.Task;
            return address;
        }

        public async UniTask<AsyncOperationHandle<T>> LoadAssetAsync<T>(AssetReference reference)
        {
            var address = Addressables.LoadAssetAsync<T>(reference);
            await address.Task;
            return address;
        }

        public async UniTask<AsyncOperationHandle<IList<T>>> LoadAssetsAsync<T>(List<string> tags, Action<T> completed)
        {
            if (tags.Count == 0)
            {
                return new AsyncOperationHandle<IList<T>>();
            }
            
            if (tags.Count == 1)
            {
                var tag = tags[0];
                var addresses = Addressables.LoadAssetsAsync<T>(tag, completed);
                await addresses.Task;
                return addresses;
            }
            
            var values = Addressables.LoadAssetsAsync<T>(tags, completed);
            await values.Task;
            return values;
        }

        public async void CheckAndDownloadRemoteAsset()
        {
            this.SendEvent<StartCheckAndUpdateAssetEvent>();
            
            // 检测更新
            var checkHandle = Addressables.CheckForCatalogUpdates(true);
            await checkHandle;
            if (checkHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.Log("CheckForCatalogUpdates Error\n" +  checkHandle.OperationException.ToString());
                this.SendEvent(new ErrorCheckAndUpdateAssetEvent("CheckForCatalogUpdates Error\n" +  checkHandle.OperationException.ToString()));
                return;
            }
            
            if (checkHandle.Result.Count > 0)
            {
                //更新Catalog
                var updateHandle = Addressables.UpdateCatalogs(checkHandle.Result, true);
                await updateHandle;

                if (updateHandle.Status != AsyncOperationStatus.Succeeded)
                {
                    Debug.Log("UpdateCatalogs Error\n" + updateHandle.OperationException.ToString());
                    this.SendEvent(new ErrorCheckAndUpdateAssetEvent("UpdateCatalogs Error\n" + updateHandle.OperationException.ToString()));
                    return;
                }

                // 更新列表迭代器
                var locators = updateHandle.Result;
                foreach (var locator in locators)
                {
                    // 获取待下载的文件总大小
                    var sizeHandle = Addressables.GetDownloadSizeAsync(locator);
                    await sizeHandle;
                    if (sizeHandle.Status != AsyncOperationStatus.Succeeded)
                    {
                        Debug.Log("GetDownloadSizeAsync Error\n" + sizeHandle.OperationException.ToString());
                        this.SendEvent(new ErrorCheckAndUpdateAssetEvent("GetDownloadSizeAsync Error\n" + sizeHandle.OperationException.ToString()));
                        break;
                    }

                    long totalDownloadSize = sizeHandle.Result;
                    mAddressableModel.CurrentDownloadAssetSize = totalDownloadSize;

                    Debug.Log("download size : " + totalDownloadSize);
                    if (totalDownloadSize > 0)
                    {
                        mAddressableModel.ShowProcessValue.Value = 0;
                        
                        // 下载
                        var downloadHandle = Addressables.DownloadDependenciesAsync(locator, true);
                        while (!downloadHandle.IsDone)
                        {
                            if (downloadHandle.Status == AsyncOperationStatus.Failed)
                            {
                                Debug.Log("DownloadDependenciesAsync Error\n"  + downloadHandle.OperationException.ToString());
                                this.SendEvent(new ErrorCheckAndUpdateAssetEvent("DownloadDependenciesAsync Error\n"  + downloadHandle.OperationException.ToString()));

                                return;
                            }
                            // 下载进度
                            mAddressableModel.ShowProcessValue.Value = downloadHandle.PercentComplete;

                            await UniTask.Yield();
                        }
                        if (downloadHandle.Status == AsyncOperationStatus.Succeeded)
                        {
                            this.SendEvent<EndCheckAndUpdateAssetEvent>();
                        }
                    }
                }
            }
            else
            {
                mAddressableModel.ShowProcessValue.Value = 1.0f;
                this.SendEvent<EndCheckAndUpdateAssetEvent>();
            }
        }
        
        protected override void OnInit()
        {
            mAddressableModel = this.GetModel<IAddressableModel>();
        }
        
        protected override void OnDeinit()
        {
            base.OnDeinit();
            
            foreach (var kvp in mCache)
            {
                kvp.Value.Release();
            }
            
            foreach (var kvp in mReferenceCache)
            {
                kvp.Value.Release();
            }
            
            mCache.Clear();
            mReferenceCache.Clear();
        }
    }
}