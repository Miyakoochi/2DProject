using Core.QFrameWork;
using Enemy.EnemyManager.Model;
using QFramework;
using UnityEngine;

namespace Enemy.EnemyManager
{
    public class EnemyManager : BaseController
    {
        private IEnemyManagerModel EnemyManagerModel;

        private void Awake()
        {
            EnemyManagerModel = this.GetModel<IEnemyManagerModel>();
        }

        private void FixedUpdate()
        {
            foreach (var unit in EnemyManagerModel.UpdateEnemyUnits)
            {
                
            }
        }
    }
}