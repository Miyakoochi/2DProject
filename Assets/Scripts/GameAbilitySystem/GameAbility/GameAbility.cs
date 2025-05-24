using System.Collections.Generic;
using UnityEngine;

namespace GameAbilitySystem.GameAbility
{
    public class GameAbility
    {
        #region base

        private string Name;

        private ScriptableObject DataModel;

        private List<string> tags;

        private GameEffect cooldown;

        private float cooldownTime;

        private GameEffect Cost;

        #endregion


        #region self

        public int Level;

        public bool IsActive;

        public int ActiveCount;

        public List<Object> Arguments;

        #endregion


        #region Funciton

        public bool CanActivate()
        {
            CheckCost();
            CheckTag();
            CheckCooldown();
            return true;
        }

        public bool CheckCost()
        {
            return true;
        }

        public bool CheckTag()
        {
            return true;
        }

        public bool CheckCooldown()
        {
            return true;
        }

        public void TryActivate()
        {
            if (CanActivate())
            {
                
            }
        }
        
        #endregion
    }
}