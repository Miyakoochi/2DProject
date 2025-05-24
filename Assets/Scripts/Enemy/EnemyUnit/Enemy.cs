using Core;
using Enemy.EnemyUnit.DataModel;
using ObjectPool;
using UnityEngine;

namespace Enemy.EnemyUnit
{
    public class Enemy : IGameObject, IPoolable<EnemyUnitDataModel>
    {
        public GameObject Self { get; set; }

        public Enemy()
        {
            
        }
        
        public void Set(EnemyUnitDataModel dataModel)
        {
            
        }

        public void Reset()
        {
        }
    }
}