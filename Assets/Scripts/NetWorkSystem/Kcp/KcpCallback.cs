using System;
using System.Buffers;
using System.Net;
using System.Net.Sockets.Kcp;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NetWorkSystem.Kcp
{

    public class KcpCallback : IKcpCallback
    {
        private SimpleSegManager.Kcp mKcpClint { get; set; }
        private Action<byte[]> mUdpSendAction;
        
        public KcpCallback(uint conv, Action<byte[]> udpSendAction)
        {
            mKcpClint = new SimpleSegManager.Kcp(conv, this);
            //默认快速模式
            mKcpClint.NoDelay(1, 10, 2, 1);
            
            //窗口大小 消息比较多话就需要设大一点
            //mKcpClint.WndSize(128, 256);
            
            mUdpSendAction = udpSendAction;
        }


        public void Input(byte[] data)
        {
            mKcpClint.Input(data);
        }
        public void Update()
        {
            mKcpClint.Update(DateTimeOffset.UtcNow);
        }

        public void Close()
        {
            mKcpClint.Dispose();
        }
        
        public void Output(IMemoryOwner<byte> buffer, int avalidLength)
        {
            var s = buffer.Memory.Span.Slice(0, avalidLength).ToArray();
            mUdpSendAction?.Invoke(s);
            //mUdpClient.SendAsync(s, s.Length, EndPoint);
            buffer.Dispose();
        }

        public void SendKcpAsync(byte[] datagram, int bytes)
        {
            mKcpClint.Send(datagram.AsSpan().Slice(0, bytes));
        }

        /// <summary>
        /// 真正获取数据
        /// </summary>
        /// <returns></returns>
        public async ValueTask<byte[]> ReceiveKcpAsync()
        {
            var (buffer, avalidLength) = mKcpClint.TryRecv();
            while (buffer == null)
            {
                await UniTask.Delay(10);
                //await UniTask.Yield();
                (buffer, avalidLength) = mKcpClint.TryRecv();
            }
            var s = buffer.Memory.Span.Slice(0, avalidLength).ToArray();
            return s;
        }
    }
}