using System.Collections.Generic;
using NetWorkSystem.Kcp;
using QFramework;

namespace NetWorkSystem
{
    public interface INetWorkModel : IModel
    {
        //被多个线程访问
        public Queue<string> Msgs { get; set; }

        public KcpServer LocalKcpServer { get; set; }
        public KcpClient LocalKcpClient { get; set; }
    }
    
    public class NetWorkModel : AbstractModel, INetWorkModel
    {
        
        
        protected override void OnInit()
        {
        }

        public Queue<string> Msgs { get; set; } = new();
        public KcpServer LocalKcpServer { get; set; } = null;
        public KcpClient LocalKcpClient { get; set; } = null;
    }
}