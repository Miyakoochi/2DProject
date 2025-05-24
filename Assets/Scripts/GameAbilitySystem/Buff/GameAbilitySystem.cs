using System;
using System.Collections.Generic;
using AssetSystem;
using Common;
using GameAbilitySystem.Buff.Apply.Bullet;
using GameAbilitySystem.Buff.Buff;
using GameAbilitySystem.Buff.Skill;
using GameAbilitySystem.Buff.TimeLine;
using LevelSystem;
using QFramework;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameAbilitySystem.Buff
{
    public interface IGameAbilitySystem : ISystem
    {
        public void LoadAllAbilitySystemDataModel();
        public TimeLineDataModel GetTimeLineDataModel(string id);
        public SkillDataModel GetSkillDataModel(string id);
        public BuffDataModel GetBuffDataModel(string id);
        public BulletDataModel GetBulletDataModel(string id);
    }
    
    public class GameAbilitySystem : AbstractSystem, IGameAbilitySystem
    {
        private IAddressableSystem mAddressableSystem;
        
        private AsyncOperationHandle<IList<SkillDataModel>> mSkillDataModelMapsHandle;
        private Dictionary<string, SkillDataModel> mSkillDataModels { get; set; } = new(StringComparer.OrdinalIgnoreCase);
        
        private AsyncOperationHandle<IList<TimeLineDataModel>> mTimeLineDataModelMapsHandle;
        private Dictionary<string, TimeLineDataModel> mTimeLineDataModels { get; set; } = new(StringComparer.OrdinalIgnoreCase);
        
        private AsyncOperationHandle<IList<BuffDataModel>> mBuffDataModelMapsHandle;
        private Dictionary<string, BuffDataModel> mBuffDataModels { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        private AsyncOperationHandle<IList<BulletDataModel>> mBulletDataModelMapsHandle;
        private Dictionary<string, BulletDataModel> mBulletDataModels { get; set; } = new(StringComparer.OrdinalIgnoreCase);
        
        public async void LoadAllAbilitySystemDataModel()
        {
            mSkillDataModels.Clear();
            mTimeLineDataModels.Clear();
            mBuffDataModels.Clear();
            mBulletDataModels.Clear();
            
            mBuffDataModelMapsHandle = await mAddressableSystem.LoadAssetsAsync<BuffDataModel>(new List<string>(){Util.BuffDataModelTag}, null);
            if (mBuffDataModelMapsHandle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var buffDataModel in mBuffDataModelMapsHandle.Result)
                {
                    mBuffDataModels.Add(buffDataModel.Id, buffDataModel);
                }
            } 
            
            mTimeLineDataModelMapsHandle = await mAddressableSystem.LoadAssetsAsync<TimeLineDataModel>(new List<string>(){Util.TimeLineDataModelTag}, null);
            if (mTimeLineDataModelMapsHandle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var timeLineDataModel in mTimeLineDataModelMapsHandle.Result)
                {
                    mTimeLineDataModels.Add(timeLineDataModel.Id, timeLineDataModel);
                }
            } 
            
            mSkillDataModelMapsHandle = await mAddressableSystem.LoadAssetsAsync<SkillDataModel>(new List<string>(){Util.SkillDataModelTag}, null);
            if (mSkillDataModelMapsHandle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var skillDataModel in mSkillDataModelMapsHandle.Result)
                {
                    mSkillDataModels.Add(skillDataModel.Id, skillDataModel);
                }
            }

            mBulletDataModelMapsHandle = await mAddressableSystem.LoadAssetsAsync<BulletDataModel>(new List<string>(){Util.BulletDataModelTag}, null);
            if (mBulletDataModelMapsHandle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var bulletDataModel in mBulletDataModelMapsHandle.Result)
                {
                    mBulletDataModels.Add(bulletDataModel.Id, bulletDataModel);
                }
            }
            
            if (mBuffDataModelMapsHandle.Status == AsyncOperationStatus.Succeeded
                && mSkillDataModelMapsHandle.Status == AsyncOperationStatus.Succeeded
                && mTimeLineDataModelMapsHandle.Status == AsyncOperationStatus.Succeeded
                && mBulletDataModelMapsHandle.Status == AsyncOperationStatus.Succeeded)
            {
                this.SendEvent<FinishedLoadAllBuffDataModel>();
            }
        }

        public TimeLineDataModel GetTimeLineDataModel(string id)
        {
            mTimeLineDataModels.TryGetValue(id, out var value);
            return value;
        }
        
        public SkillDataModel GetSkillDataModel(string id)
        {
            mSkillDataModels.TryGetValue(id, out var value);
            return value;
        }
        
        public BuffDataModel GetBuffDataModel(string id)
        {
            mBuffDataModels.TryGetValue(id, out var value);
            return value;
        }

        public BulletDataModel GetBulletDataModel(string id)
        {
            mBulletDataModels.TryGetValue(id, out var value);
            return value;
        }
        
        protected override void OnInit()
        {
            mAddressableSystem = this.GetSystem<IAddressableSystem>();
        }
    }
}