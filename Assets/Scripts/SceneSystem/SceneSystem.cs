using Common;
using Cysharp.Threading.Tasks;
using DataModelManager;
using LevelSystem;
using Player.PlayerManager;
using QFramework;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace SceneSystem
{
    public interface ISceneSystem : ISystem
    {
        /// <summary>
        /// 加载资产
        /// </summary>
        /// <param name="mapId"></param>
        /// <param name="playerId"></param>
        public void LoadGameScene(int mapId, int playerId);

        public void ReturnMainMenu();
    }
    
    public class SceneSystem : AbstractSystem, ISceneSystem
    {
        private IPlayerSystem mPlayerSystem;
        private ILevelSystem mLevelSystem;
        private IDataModelSystem mDataModelSystem;
        
        public async void LoadGameScene(int mapId, int playerId)
        {
            if (SceneManager.GetActiveScene().name == SceneUtil.GameScene)
            {
                return;
            }

            this.SendEvent<SceneStartLoadEvent>();
            
            //加载关卡数据与预制件，用于初始化。
            var level = await mLevelSystem.LoadLevelDataModel();
            mLevelSystem.CreateLevelMap(mapId);
            
            //加载玩家数据，用于初始化
            var player = await mPlayerSystem.LoadAllPlayerDataModel();
            
            if (level.Status != AsyncOperationStatus.Succeeded ||
                player.Status != AsyncOperationStatus.Succeeded) return;
            
            //加载场景
            var sceneAsync = SceneManager.LoadSceneAsync(SceneUtil.GameScene, LoadSceneMode.Single);
            if (sceneAsync != null)
            {
                await sceneAsync;
                sceneAsync.allowSceneActivation = false;
            }

            if (sceneAsync != null)
            {
                sceneAsync.allowSceneActivation = true;
                this.SendEvent<SceneEndLoadEvent>();
            }
        }

        public async void ReturnMainMenu()
        {
            mLevelSystem.ClearLevelMap();
            
            if (SceneManager.GetActiveScene().name == SceneUtil.MainMenuScene)
            {
                return;
            }

            //加载场景
            var sceneAsync = SceneManager.LoadSceneAsync(SceneUtil.MainMenuScene, LoadSceneMode.Single);
            if (sceneAsync != null)
            {
                await sceneAsync;
                sceneAsync.allowSceneActivation = false;
            }

            if (sceneAsync != null)
            {
                sceneAsync.allowSceneActivation = true;
                this.SendEvent<SceneEndLoadEvent>();
            }
        }

        protected override void OnInit()
        {
            mPlayerSystem = this.GetSystem<IPlayerSystem>();
            mLevelSystem = this.GetSystem<ILevelSystem>();
        }
    }
}