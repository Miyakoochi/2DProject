using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Player.Data
{
    [CreateAssetMenu(menuName = "Player/Data/PlayerData", fileName = "PlayerDataModel")]
    public class PlayerDataModel : ScriptableObject
    {
        public int id;
        public float MoveSpeed;
        public RuntimeAnimatorController PlayerAnimator;
        public InputActionAsset PlayerInputAction;
    }
}