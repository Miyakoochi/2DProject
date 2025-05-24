using UnityEngine;

namespace InputSystem
{
    public interface ICanController
    {
        public void OnUnitFire();

        public void OnUnitStop();

        public void OnUnitMove(Vector2 direction);
    }
}