using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace C2DGame.Networking
{
    public class C2DPlayer : NetworkBehaviour
    {
        public Character2D player;  // local player

        private void Update()
        {
            if (isLocalPlayer)
                transform.position = player.transform.position;
        }
    }
}
