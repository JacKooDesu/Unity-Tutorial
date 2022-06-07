using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

namespace C2DGame.Networking
{
    public class C2DPlayer : NetworkBehaviour
    {
        public Character2D player;  // local player
        public NetworkTransform networkTransform;

        [SyncVar(hook = nameof(OnPlayerNameChanged))] string playerName;
        void OnPlayerNameChanged(string _, string newName)
        {
            playerName = newName;
            UpdateName(playerName);
        }

        public override void OnStartClient()
        {
            networkTransform = GetComponent<NetworkTransform>();
            if (hasAuthority)
            {
                player = FindObjectOfType<Character2D>();
                GetComponent<SpriteRenderer>().enabled = false;

                CmdChangeName(((C2DNetworkManager)NetworkRoomManager.singleton).GetLocalRoomPlayer().playerName);
                GetComponentInChildren<Text>().gameObject.SetActive(false);
            }
        }

        [Command]
        void CmdChangeName(string name)
        {
            playerName = name;
        }

        private void Update()
        {
            if (player != null)
                transform.position = player.transform.position;
        }

        public void UpdateName(string name)
        {
            GetComponentInChildren<Text>().text = name;
        }
    }
}
