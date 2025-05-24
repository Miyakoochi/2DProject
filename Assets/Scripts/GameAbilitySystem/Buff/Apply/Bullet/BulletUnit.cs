using Common;
using Core;
using Core.QFrameWork;
using GameAbilitySystem.Buff.Unit;
using ObjectPool;
using QFramework;
using UnityEngine;

namespace GameAbilitySystem.Buff.Apply.Bullet
{
    public class BulletUnit : IGameObject, IPoolable<BulletDataModel>
    {
        public Transform SelfTransform;
        private SpriteRenderer mRenderer;
        public Rigidbody2D SelfRigidbody;
        public CircleCollider2D SelfCollider2D;
        private BulletCollision mBulletCollision;
        public BulletDataModel DataModel;
        
        public IGameAbilityUnit Owner;
        public Vector3 MoveDirection;
        public float Speed;
        public float Duration = 0;
        
        public BulletUnit()
        {
            Self = new GameObject("BulletUnit");
            SelfTransform = Self.transform;
            
            mRenderer = Self.AddComponent<SpriteRenderer>();
            mRenderer.sortingLayerName = SortingLayerName.ApplyLayer;
            SelfRigidbody = Self.AddComponent<Rigidbody2D>();
            SelfRigidbody.gravityScale = 0;
            
            SelfCollider2D = Self.AddComponent<CircleCollider2D>();
            SelfCollider2D.isTrigger = true;

            mBulletCollision = Self.AddComponent<BulletCollision>();
            mBulletCollision.Owner = this;
            
            Self.layer = LayerMask.NameToLayer("Apply");;
        }

        public GameObject Self { get; set; }
        
        public void Set(BulletDataModel dataModel)
        {
            DataModel = dataModel;
            mRenderer.sprite = dataModel.sprite;
            SelfCollider2D.radius = dataModel.Radius;
            Speed = dataModel.Speed;
            Duration = dataModel.Duration;
        }

        public void Reset()
        {
            DataModel = null;
            mRenderer.sprite = null;
            Speed = 0;
            Duration = 0;
        }
    }
}