using Core.QFrameWork;
using ObjectPool;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public class ArrowManager : BaseController
    {
        public GameObject CursorTexture; // 拖入自定义鼠标 Sprite

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
 
        private void FixedUpdate()
        {
            // 将鼠标屏幕坐标转换为世界坐标
            if (Camera.main && CursorTexture)
            {
                Cursor.visible = false;
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0; // 确保 Z 轴为 0（2D 场景）
                CursorTexture.transform.position = mousePos;
            }
        }
    }
}