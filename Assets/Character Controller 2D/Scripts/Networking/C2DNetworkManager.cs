using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using C2DGame.System;

namespace C2DGame.Networking
{
    public class C2DNetworkManager : NetworkRoomManager
    {
        public override void OnClientConnect()
        {
            base.OnClientConnect();
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
        }

        public override void OnClientDisconnect()
        {
            GameHandler.Singleton.ProgressScene(GameState.IDLE);
            base.OnClientDisconnect();
        }

        public override void OnRoomClientConnect()
        {
            base.OnRoomClientConnect();
            print("客戶端執行 | 連入");
        }
    }
}

