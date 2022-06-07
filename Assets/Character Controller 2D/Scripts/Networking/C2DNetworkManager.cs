using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using C2DGame.GameSystem;

namespace C2DGame.Networking
{
    public class C2DNetworkManager : NetworkRoomManager
    {
        public GameObject netLevelManager;
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
            // 清除離線玩家
            if (conn.authenticationData != null)
                C2DNetworkAuthenticator.playerNames.Remove((string)conn.authenticationData);

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

        public override void OnRoomServerPlayersReady()
        {
            //base.OnRoomServerPlayersReady();
        }

        public override void OnRoomServerSceneChanged(string scene)
        {
            if (scene == GameplayScene)
            {
                NetworkServer.Spawn(Object.Instantiate(netLevelManager));
            }
        }

        #region Utils

        public C2DRoomPlayer GetLocalRoomPlayer()
        {
            foreach (var p in roomSlots)
            {
                if (p.isLocalPlayer)
                    return p as C2DRoomPlayer;
            }

            return null;
        }
        #endregion
    }
}

