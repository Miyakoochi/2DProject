namespace NetWorkSystem
{
    public struct InitNetWorkEvent
    {
        
    }
    
    public struct ConnectRoomEvent
    {
        public string ip;
        public int port;
    }
    
    public struct CreateRoomEvent
    {
        public int port;
    }

    public struct CloseRoomEvent
    {
        
    }

    public struct ConnectSuccessEvent
    {
        
    }

    public struct UpgradeFailure
    {
        
    }
    
    //当客户端成功连接服务端后发送事件
    public struct ClientConnectSuccessEvent
    {
        
    }
}