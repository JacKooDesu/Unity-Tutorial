using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FileManagerTutorial
{
    public class PlayerObject : MonoBehaviour
    {
        public PlayerState state;

        [ContextMenu("存檔 - state")]
        public void SaveState()
        {
            FileManager<PlayerState>.Save("PlayerData", state, "Save");
        }

        [ContextMenu("存檔 - mono")]
        public void SaveMono()
        {
            FileManager<PlayerObject>.Save("PlayerDataMono", this, "Save");
        }

        [ContextMenu("讀檔 - state")]
        public void LoadState()
        {
            FileManager<PlayerState>.Load("Save", "PlayerData", state);
        }

        [ContextMenu("讀檔 - mono")]
        public void LoadMono()
        {
            FileManager<PlayerObject>.Load("Save", "PlayerDataMono", this);
        }
    }
}
