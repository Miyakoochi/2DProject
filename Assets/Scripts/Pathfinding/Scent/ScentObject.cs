using Core;
using Core.QFrameWork;
using Cysharp.Threading.Tasks;
using ObjectPool;
using QFramework;
using UnityEngine;

namespace Pathfinding.Scent
{
    public class ScentObject :IGameObject, IPoolable<ScentDataModel>, IController
    {
        private SpriteRenderer mSpriteRenderer;
        
        public ScentObject()
        {
            Self = new GameObject("Scent");
            mSpriteRenderer = Self.AddComponent<SpriteRenderer>();
        }
        
        public void Set(ScentDataModel dataModel)
        {
            if (dataModel.Sprite)
            {
                mSpriteRenderer.sprite = dataModel.Sprite;
            }
        }

        public void Reset()
        {
            
        }

        private async void DestroySelf()
        {
            await UniTask.WaitForSeconds(1);
            this.GetSystem<IObjectPoolSystem>().ReleaseObject(this);
        }
        

        public GameObject Self { get; set; }
        public IArchitecture GetArchitecture()
        {
            return GameContext.Interface;
        }
    }
}