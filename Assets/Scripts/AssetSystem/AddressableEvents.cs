namespace AssetSystem
{
    public struct StartCheckAndUpdateAssetEvent
    {
        
    }

    public struct ErrorCheckAndUpdateAssetEvent
    {
        public ErrorCheckAndUpdateAssetEvent(string message)
        {
            Message = message;
        }
        
        public string Message;
    }

    public struct EndCheckAndUpdateAssetEvent
    {
        
    }
}