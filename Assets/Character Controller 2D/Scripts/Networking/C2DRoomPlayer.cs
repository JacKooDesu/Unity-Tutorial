using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

namespace C2DGame.Networking
{
    public class C2DRoomPlayer : NetworkRoomPlayer
    {
        static UI.RoomUiHandler roomUi;

        [SyncVar(hook = nameof(OnPlayerNameChanged))] public string playerName;
        void OnPlayerNameChanged(string _, string newName)
        {
            playerName = newName;
            if (roomUi == null)
                roomUi = FindObjectOfType<UI.RoomUiHandler>();

            roomUi.UpdatePlayerList();
        }

        public override void OnClientEnterRoom()
        {
            // 進入房間才執行，避免過早執行物件還未註冊
            if (roomUi == null)
                roomUi = FindObjectOfType<UI.RoomUiHandler>();
            if (roomUi != null)
                roomUi.UpdatePlayerList();

            base.OnStartClient();
        }

        public override void OnClientExitRoom()
        {
            // 進入房間才執行，避免過早執行物件還未註冊
            if (roomUi == null)
                roomUi = FindObjectOfType<UI.RoomUiHandler>();

            if (roomUi != null)
                roomUi.UpdatePlayerList();

            base.OnClientExitRoom();
        }

        public override void OnStartServer()
        {
            playerName = (string)connectionToClient.authenticationData;
            readyToBegin = true;
        }

        void BindUi()
        {
            // 檢查是否在Room Scene(room ui handler不存在代表不是)
            if (roomUi == null)
            {
                UI.RoomUiHandler ui;
                if ((ui = FindObjectOfType<UI.RoomUiHandler>()) == null)
                    return;

                roomUi = ui;
            }
        }
    }

}
