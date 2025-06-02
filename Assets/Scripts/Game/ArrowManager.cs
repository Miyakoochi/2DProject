using Core.QFrameWork;
using InputSystem;
using QFramework;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class ArrowManager : BaseController
    {
        public GameObject CursorTexture; // 拖入自定义鼠标 Sprite

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            this.GetSystem<IInputControllerSystem>().BindMousePosition(OnMousePosition);
            Cursor.visible = false;
        }

        private void OnMousePosition(InputAction.CallbackContext callbackContext)
        {
            if(!CursorTexture) return;
            
            var position = callbackContext.ReadValue<Vector2>();
            CursorTexture.transform.position = position;
        }
    }
}