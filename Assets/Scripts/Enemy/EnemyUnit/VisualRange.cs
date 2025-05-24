using Core.QFrameWork;
using UnityEngine;

namespace Enemy.EnemyUnit
{
    public class VisualRange : BaseController
    {
        public EnemyUnit Unit;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (Unit)
                {
                    //Debug.Log("Target Enter");
                    Unit.Target = other.transform;
                }
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (Unit)
                {
                    //Debug.Log("Target Exit");
                    Unit.Target = null;
                }
            }
            
        }
    }
}