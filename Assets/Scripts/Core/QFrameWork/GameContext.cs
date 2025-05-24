using System;
using System.Linq;
using AssetSystem;
using Audio;
using CsLua;
using Enemy.EnemyManager.Model;
using Enemy.EnemyManager.System;
using GameAbilitySystem.Buff;
using GameAbilitySystem.Buff.Manager;
using GameAbilitySystem.Buff.Unit;
using InputSystem;
using LevelSystem;
using NetWorkSystem;
using NetWorkSystem.Kcp;
using ObjectPool;
using Pathfinding;
using Player.PlayerManager;
using QFramework;
using SceneSystem;
using UI.UICore;

namespace Core.QFrameWork
{
    public class GameContext : Architecture<GameContext>
    {
        protected override void Init()
        {
            RegisterModel<IAddressableModel>(new AddressableModel());
            RegisterModel<IBulletManagerModel>(new BulletManagerModel());
            RegisterModel<IEnemyManagerModel>(new EnemyManagerModel());
            RegisterModel<IPlayerModel>(new PlayerModel());
            RegisterModel<ILevelModel>(new LevelModel());
            RegisterModel<INetWorkModel>(new NetWorkModel());
            RegisterModel<IUIModel>(new UIModel());
            RegisterModel<IUnitModel>(new UnitModel());
            RegisterModel<ITimeLineModel>(new TimeLineModel());
            RegisterModel<IDamageModel>(new DamageModel());
            RegisterModel<IPathModel>(new PathModel());
            RegisterModel<IAudioModel>(new AudioModel());
            
            RegisterSystem<IInputControllerSystem>(new InputControllerSystem());
            RegisterSystem<IBulletManagerSystem>(new BulletManagerSystem());
            RegisterSystem<IEnemyManagerSystem>(new EnemyManagerSystem());
            RegisterSystem<ILevelSystem>(new LevelSystem.LevelSystem());
            RegisterSystem<IAddressableSystem>(new AddressableSystem());
            RegisterSystem<IPlayerSystem>(new PlayerSystem());
            RegisterSystem<ISceneSystem>(new SceneSystem.SceneSystem());
            RegisterSystem<INetWorkSystem>(new NetWorkSystem.NetWorkSystem());
            RegisterSystem<IUISystem>(new UISystem());
            RegisterSystem<IUnitSystem>(new UnitSystem());
            RegisterSystem<ITimeLineSystem>(new TimeLineSystem());
            RegisterSystem<IGameAbilitySystem>(new GameAbilitySystem.Buff.GameAbilitySystem());
            RegisterSystem<IDamageSystem>(new DamageSystem());
            RegisterSystem<ILuaSystem>(new LuaSystem());
            RegisterSystem<IObjectPoolSystem>(new ObjectPoolSystem());
            RegisterSystem<IPathSystem>(new PathSystem());
            RegisterSystem<IAudioSystem>(new AudioSystem());
        }
    }
}