using System;
using System.Collections.Generic;
using Enemy.EnemyUnit.DataModel;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Enemy.EnemySpawner.DataModel
{
    [Serializable]
    public struct Spawner
    {
        public AssetReference EnemyPrefab;
        public int Nums;
    }
    
    [CreateAssetMenu(menuName = "Unit/EnemyUnitSpawnerData")]
    public class EnemyUnitSpawnerDataModel : ScriptableObject
    {
        public List<EnemyUnitDataModel> EnemyUnitDataModels;
        public List<Spawner> EnemyUnits;
        public int Turns;
    }
}