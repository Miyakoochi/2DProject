using Audio;
using Command;
using Core.QFrameWork;
using NetWorkSystem;
using QFramework;
using TMPro;
using UI.UICore;
using UnityEngine.UI;

namespace UI
{
    public class CreateRoomController : BaseController
    {
        public Button Delete;
        public Button CreateRoom;
        public TMP_InputField PortInputField;

        private IUISystem mUISystem;
        private INetWorkSystem mNetWorkSystem;
        
        
        private void Awake()
        {
            if (Delete)
            {
                Delete.onClick.AddListener(OnDeleteClick);
            }

            if (CreateRoom)
            {
                CreateRoom.onClick.AddListener(OnCreateRoom);
            }

            mUISystem = this.GetSystem<IUISystem>();
            mNetWorkSystem = this.GetSystem<INetWorkSystem>();
        }

        private void OnCreateRoom()
        {
            this.GetSystem<IAudioSystem>().PlayAudioOnce(EMusicType.Click);
            if (mNetWorkSystem.IsValidPort(PortInputField.text) == false)
            {
                mUISystem.ShowTips("Tip_PortError");
                return;
            }
            
            //mNetWorkSystem.CreateRoom(int.Parse(PortInputField.text));

            this.SendCommand(new CreateRoomCommand(int.Parse(PortInputField.text)));
            mUISystem.SetUIShow(UIType.CreateRoom, false);
            mUISystem.SetUIShow(UIType.Wating, true);
        }

        private void OnDeleteClick()
        {
            this.GetSystem<IAudioSystem>().PlayAudioOnce(EMusicType.Click);
            mUISystem.SetUIShow(UIType.CreateRoom, false);
        }
    }
}