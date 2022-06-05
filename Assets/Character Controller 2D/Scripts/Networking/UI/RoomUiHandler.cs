using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

namespace C2DGame.Networking.UI
{
    public class RoomUiHandler : MonoBehaviour
    {
        [SerializeField] C2DNetworkManager networkManager;

        [SerializeField] Transform playerListParent;
        [SerializeField] GameObject playerListItemPrefab;

        [Header("按鈕")]

        [SerializeField] Button loadGameBtn;
        [SerializeField] Button newGameBtn;

        public void Start()
        {
            if (networkManager == null)
                networkManager = FindObjectOfType<C2DNetworkManager>();

            BindButton();
        }

        public void UpdatePlayerList()
        {
            print($"更新玩家列表 | 目前玩家數 {networkManager.roomSlots.Count}");
            // Clear Prefabs
            for (int i = playerListParent.childCount - 1; i >= 0; --i)
                Destroy(playerListParent.GetChild(i).gameObject);

            foreach (var p in networkManager.roomSlots)
            {
                var player = p as C2DRoomPlayer;
                var pObject = Instantiate(playerListItemPrefab, playerListParent);
                pObject.GetComponent<Text>().text = player.playerName;
            }

            // foreach (var pName in C2DNetworkAuthenticator.playerNames)
            // {
            //     print($"玩家 {pName}");
            //     var pObject = Instantiate(playerListItemPrefab, playerListParent);
            //     pObject.GetComponent<Text>().text = pName;
            // }
        }

        void BindButton()
        {

        }
    }
}

