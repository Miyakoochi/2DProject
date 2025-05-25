using UnityEngine;
using UnityEngine.Serialization;

namespace UI.UICore
{
    public enum UIType
    {
        Tips,
        Loading,
        MainMenu,
        CreateRoom,
        ConnectRoom,
        ChooseNetWorkMode,
        Wating,
        ChooseLevel,
        StopGame,
        GameEndMenu,
        GameStopMenu
    }
    
    [CreateAssetMenu(menuName = "DataModel/UIDataModel", fileName = "UIDataModel")]
    public class UIDataModel : ScriptableObject
    {
        public UIType type; 
        public GameObject UIPrefabs;
    }
}