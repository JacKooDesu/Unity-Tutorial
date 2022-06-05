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
            if (roomUi == null)
                roomUi = FindObjectOfType<UI.RoomUiHandler>();

            roomUi.UpdatePlayerList();

            base.OnStartClient();
        }

        public override void OnStartServer()
        {
            playerName = (string)connectionToClient.authenticationData;
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
