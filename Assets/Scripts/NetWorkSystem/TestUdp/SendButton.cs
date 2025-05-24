using UnityEngine;
using UnityEngine.UI;

namespace NetWorkSystem.TestUdp
{
    public class SendButton : MonoBehaviour
    {
        private MyKcpClient client;
        public Button Button;
        private void Start()
        {
            client = FindObjectOfType<MyKcpClient>().GetComponent<MyKcpClient>();
            Button.onClick.AddListener(() =>
            {
                client.Send();
            });
        }
    }
}