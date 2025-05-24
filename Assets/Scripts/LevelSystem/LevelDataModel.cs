using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LevelSystem
{
    [Serializable]
    public struct MapInfo
    {
        public Texture MapImage;
    }
    
    /// <summary>
    /// 两种获取地图数据的方式
    /// 1.通过该方式导入对应地图的资产
    /// 2.通过表格导入地图的Label或者是资产Path
    /// </summary>
    [CreateAssetMenu(menuName = "DataModel/LevelDataModel", fileName = "LevelDataModel")]
    public class LevelDataModel : ScriptableObject
    {
        public int MapId;
        public GameObject MapPrefab;
        public MapInfo MapInfo;
    }
}