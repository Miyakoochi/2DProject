using System;
using UnityEngine;

namespace NetWorkSystem
{
    public enum MsgType
    {
        Ping = 0,
        UpgradeRequestC2S = 1,
        UpgradeSuccessS2C = 2,
        ConnectRequire = 3,
        ConnectSuccess = 4,
        UpgradeEndC2S = 5, 
        GameStart = 6,
        PlayerMoveSpeed = 7,
        UpgradeFailure = 8,
        StartGameRequest = 9,
        StartGameSuccess = 10
    }

    [Serializable]
    public class BaseMsg
    {
        public MsgType MsgType;
    }

    [Serializable]
    public class BaseKcpMsg : BaseMsg
    {
        public uint Conv;
    }
    
    [Serializable]
    public class PingMsg : BaseKcpMsg
    {
        public long PingTime;
    }
    
    [Serializable]
    public class UpgradeRequestMsg : BaseKcpMsg
    {

    }
    
    [Serializable]
    public class UpgradeSuccessMsg : BaseKcpMsg
    {

    }
    
    [Serializable]
    public class UpgradeEndMsg : BaseKcpMsg
    {
        
    }
    
    [Serializable]
    public class ConnectRequireMsg : BaseMsg
    {
        
    }
    
    [Serializable]
    public class ConnectSuccessMsg : BaseMsg
    {
        
    }
    
    [Serializable]
    public class GameStartMsg : BaseKcpMsg
    {

    }
    
    [Serializable]
    public class PlayerMoveSpeedMsg : BaseKcpMsg
    {
        public Vector2 MoveVelocity;
    }
    
    [Serializable]
    public class StartGameRequestMsg : BaseKcpMsg
    {
        
    }
}