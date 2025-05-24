using System;
using Core.QFrameWork;
using Cysharp.Threading.Tasks;
using TMPro;

namespace NetWorkSystem.TestUdp
{
    public class TestPing : BaseController
    {
        public TextMeshProUGUI mPingText;
        private MyKcpClient client;
        private void Awake()
        {
            mPingText = GetComponent<TextMeshProUGUI>();
            client = FindObjectOfType<MyKcpClient>().GetComponent<MyKcpClient>();
        }

        private void Start()
        {
            Run();
        }

        private async void Run()
        {
            try
            {
                while (true)
                {
                    await UniTask.Delay(1000);
                    mPingText.text = client.GetRTT().ToString();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}