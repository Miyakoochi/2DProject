using Core.QFrameWork;
using DG.Tweening;
using LevelSystem;
using Player.PlayerManager;
using QFramework;
using UnityEngine;

namespace Player.Player
{
    public class CameraToPlayerController : BaseController
    {
        private IPlayerModel mPlayerModel;
        private ILevelModel mLevelModel;

        [Header("振动参数")]
        public float shakeDuration = 0.2f;  // 振动持续时间
        public float shakeStrength = 0.1f;  // 振动强度
        public int shakeVibrato = 10;       // 振动频率
        public float shakeRandomness = 90f; // 随机性
        private bool mIsShaking = false;             // 是否正在振动

        public Transform CameraMainTransform;
        private Camera mCam;
        private float mHalfHeight;
        private float mHalfWidth;

        private void Awake()
        {
            mPlayerModel = this.GetModel<IPlayerModel>();
            mLevelModel = this.GetModel<ILevelModel>();
            this.RegisterEvent<PlayerFireEvent>(OnPlayerFire);
        }
        

        void Start()
        {
            mCam = GetComponentInChildren<Camera>();
            
            // 计算摄像机视口的一半高度和宽度（基于正交摄像机）
            mHalfHeight = mCam.orthographicSize;
            mHalfWidth = mHalfHeight * mCam.aspect; // 根据屏幕宽高比计算宽度
            CameraMainTransform.localPosition = new Vector3(0.0f, 0.0f, -10.0f);
        }
        
        void LateUpdate()
        {
            if (mPlayerModel == null) return;
            if(mLevelModel == null) return;
            // 获取目标位置
            Vector3 targetPosition = mPlayerModel.CurrentControlPlayer.Owner.transform.position;
            
            // 保持摄像机原有Z轴位置（避免移动到角色Z轴位置）
            targetPosition.z = transform.position.z;
            
            // 计算摄像机位置在边界内的限制值
            float clampedX = Mathf.Clamp(
                targetPosition.x,
                mLevelModel.LevelBoundLeftDown.x + mHalfWidth,   // 左边界 + 摄像机半宽
                mLevelModel.LevelBoundRightUp.x - mHalfWidth    // 右边界 - 摄像机半宽
            );
            float clampedY = Mathf.Clamp(
                targetPosition.y,
                mLevelModel.LevelBoundLeftDown.y + mHalfHeight,  // 下边界 + 摄像机半高
                mLevelModel.LevelBoundRightUp.y - mHalfHeight   // 上边界 - 摄像机半高
            );
            // 应用限制后的位置
            transform.position = new Vector3(clampedX, clampedY, targetPosition.z);
        }
        
        public void ShakeCamera()
        {
            if (mIsShaking && CameraMainTransform == null) return;
        
            mIsShaking = true;
            
            // 使用DoTween的DOShakePosition实现振动效果
            CameraMainTransform.DOShakePosition(
                duration: shakeDuration,
                strength: shakeStrength,
                vibrato: shakeVibrato,
                randomness: shakeRandomness,
                snapping: false
            ).OnComplete(() => 
            {
                // 振动完成后重置位置
                CameraMainTransform.localPosition = new Vector3(0.0f, 0.0f, -10.0f);
                mIsShaking = false;
            });
        }
        
        private void OnPlayerFire(PlayerFireEvent obj)
        {
            ShakeCamera();
        }

    }
}