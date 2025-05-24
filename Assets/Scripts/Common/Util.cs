namespace Common
{
    public static class EnemyUtil
    {
        public static readonly string EnemyPrefabPath = "Assets/Prefab/Enemy/EnemyUnit/EnemyUnit.prefab";
    }
    
    public static class Util
    {
        public static readonly string PlayerDataModelTag = "PlayerDataModel";
        public static readonly string LevelDataModelTag = "LevelDataModel";
        public static readonly string UIDataModelTag = "UIDataModel";
        public static readonly string BuffDataModelTag = "BuffDataModel";
        public static readonly string SkillDataModelTag = "SkillDataModel";
        public static readonly string TimeLineDataModelTag = "TimeLineDataModel";
        public static readonly string BulletDataModelTag = "BulletDataModel";
        public static readonly string EnemyDataModelTag = "EnemyDataModel";
    }

    public static class SceneUtil
    {
        public static readonly string MainMenuScene = "MainMenu";
        public static readonly string GameScene = "GameScene";
        public static readonly string GuideScene = "Guide";
        public static readonly string LoadingScene = "LoadingScene";
    }

    public static class NetWorkUtil
    {
        public static string ServerDefaultIpAddress = "127.0.0.1";
    }

    public static class AddressableUtil
    {
        public static string AddressableRemotePathUrl = "https://gitee.com/eureka77233/ProjectAssetBundle/raw/main";
        public static string AddressableRemoteLoadPath = "ServerData";
        public static string AddressableRemoteBuildPath = "ServerDataBuild";
    }

    public static class SortingLayerName
    {
        public static string UnitLayer = "Unit";
        public static string ApplyLayer = "Apply";
        public static string PlayerLayer = "Player";
    }
}