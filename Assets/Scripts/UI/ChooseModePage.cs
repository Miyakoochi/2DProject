using System.Collections;
using System.Collections.Generic;
using Audio;
using Core.QFrameWork;
using QFramework;
using UI.UICore;
using UnityEngine;
using UnityEngine.UI;

public class ChooseModePage : BaseController
{
    public Button BackButton;
    public Button CreateRoomButton;
    public Button ConnectRoomButton;

    private IUISystem mUISystem;
    private void Awake()
    {
        BackButton.onClick.AddListener(OnBackButtonClick);
        CreateRoomButton.onClick.AddListener(OnCreateRoomButtonClick);
        ConnectRoomButton.onClick.AddListener(OnConnectButtonClick);

        mUISystem = this.GetSystem<IUISystem>();
    }

    private void OnConnectButtonClick()
    {
        this.GetSystem<IAudioSystem>().PlayAudioOnce(EMusicType.Click);
        mUISystem.SetUIShow(UIType.ConnectRoom, true);
        mUISystem.SetUIShow(UIType.ChooseNetWorkMode, false);
    }

    private void OnCreateRoomButtonClick()
    {
        this.GetSystem<IAudioSystem>().PlayAudioOnce(EMusicType.Click);
        mUISystem.SetUIShow(UIType.CreateRoom, true);
        mUISystem.SetUIShow(UIType.ChooseNetWorkMode, false);
    }

    private void OnBackButtonClick()
    {
        this.GetSystem<IAudioSystem>().PlayAudioOnce(EMusicType.Click);
        mUISystem.SetUIShow(UIType.ChooseNetWorkMode, false);
    }
}
