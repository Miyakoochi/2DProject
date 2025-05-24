using UnityEngine;

namespace Enemy.EnemyUnit.DataModel
{
    [CreateAssetMenu(menuName = "Unit/EnemyDataModel")]
    public class EnemyUnitDataModel : ScriptableObject
    {
        public int Id;
        public GameObject EnemyPrefab;
    }
}
