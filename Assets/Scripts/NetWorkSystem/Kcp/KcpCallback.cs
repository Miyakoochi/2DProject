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
        private bool mIsClosed = false;
        public KcpCallback(uint conv, Action<byte[]> udpSendAction)
        {
            mKcpClint = new SimpleSegManager.Kcp(conv, this);
            //默认快速模式
            mKcpClint.NoDelay(1, 20, 2, 1);
            
            //窗口大小 消息比较多话就需要设大一点
            mKcpClint.WndSize(64, 128);
            
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
            mIsClosed = true;
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
            try
            {
                mKcpClint.Send(datagram.AsSpan().Slice(0, bytes));
            }
            catch (ObjectDisposedException)
            {
            }
        }

        /// <summary>
        /// 真正获取数据
        /// </summary>
        /// <returns></returns>
        public async ValueTask<byte[]> ReceiveKcpAsync()
        {
            try
            {
                var (buffer, avalidLength) = mKcpClint.TryRecv();
                while (buffer == null)
                {
                    await UniTask.Yield();
                    //await UniTask.Yield();
                    (buffer, avalidLength) = mKcpClint.TryRecv();
                }

                var s = buffer.Memory.Span.Slice(0, avalidLength).ToArray();
                return s;
            }
            catch (NullReferenceException)
            {
                return Array.Empty<byte>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}