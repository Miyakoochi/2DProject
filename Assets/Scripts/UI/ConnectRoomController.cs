using System;
using Command;
using Core.QFrameWork;
using NetWorkSystem;
using QFramework;
using TMPro;
using UI.UICore;
using UnityEngine.UI;

namespace UI
{
    public class ConnectRoomController : BaseController
    {
        public Button Delete;
        public Button ConnectRoom;

        public TMP_InputField IpAddress;
        public TMP_InputField Port;
        
        private IUISystem mUISystem;
        private INetWorkSystem mNetWorkSystem;
        
        private void Awake()
        {
            if (Delete)
            {
                Delete.onClick.AddListener(OnDeleteClick);
            }

            if (ConnectRoom)
            {
                ConnectRoom.onClick.AddListener(OnConnectRoom);
            }

            mUISystem = this.GetSystem<IUISystem>();
            mNetWorkSystem = this.GetSystem<INetWorkSystem>();
        }

        private void OnConnectRoom()
        {
            var text = IpAddress.text;
            
            if (mNetWorkSystem.IsValidIP(text) == false)
            {
                IpAddress.text = string.Empty;
                mUISystem.ShowTips("Tip_IpError");
                return;
            }

            if (mNetWorkSystem.IsValidPort(Port.text) == false)
            {
                Port.text = string.Empty;
                mUISystem.ShowTips("Tip_PortError");
                return;
            }
            
            mUISystem.SetUIShow(UIType.Wating, true);
            mUISystem.SetUIShow(UIType.ConnectRoom, false);

            this.SendCommand(new ConnectRoomCommand(IpAddress.text, int.Parse(Port.text)));
        }

        private void OnDeleteClick()
        {
            mUISystem.SetUIShow(UIType.ConnectRoom, false);
        }
    }
}