using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace C2DGame.Networking
{
    public class C2DNetworkLevelManager : NetworkBehaviour
    {
        public SyncList<LevelData.StageData> syncStageDatas = new SyncList<LevelData.StageData>();
        LevelGenerator levelGenerator;

        public override void OnStartClient()
        {
            base.OnStartClient();
            levelGenerator = GetComponent<LevelGenerator>();
            if (GameSystem.GameHandler.gameState == GameSystem.GameState.NETWORK_SERVER)
                CoroutineUtility.Manager.Singleton.Add(levelGenerator.Generate(), LevelBuildComplete, "BuildLevel");

            Time.timeScale = 0f;
        }

        void LevelBuildComplete()
        {
            var levelData = levelGenerator.ToLevelData();
            syncStageDatas.AddRange(levelData.stageDatas);
            RpcBuildStage();
        }

        [ClientRpc]
        public async void RpcBuildStage()
        {
            while(syncStageDatas.Count==0)
                await System.Threading.Tasks.Task.Yield();
            
            if (!isServer)
            {
                var levelData = new List<LevelData.StageData>();
                levelData.AddRange(syncStageDatas);
                levelGenerator.LoadLevel(levelData);
            }
            Time.timeScale = 1f;
        }
    }
}
