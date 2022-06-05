﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using C2DGame.Networking;

namespace C2DGame.System
{
    public class GameHandler : MonoBehaviour
    {
        static GameHandler singleton = null;
        public static GameHandler Singleton
        {
            get
            {
                if (singleton != null)
                    return singleton;

                singleton = FindObjectOfType<GameHandler>();

                if (singleton == null)
                {
                    GameObject g = new GameObject("GameHandler");
                    singleton = g.AddComponent<GameHandler>();
                }

                return singleton;
            }
        }

        public static GameState gameState;
        public static string targetIp;

        public static string PlayerName { set; get; }

        public C2DNetworkManager manager;

        #region Scene Names
        const string SCENE_IDLE = "Menu";
        const string SCENE_LOCAL = "C2D Menu Local";
        const string SCENE_NETWORK = "C2D Menu Network";
        #endregion

        private void Awake()
        {
            if (singleton != null && singleton != this)
            {
                Destroy(gameObject);
                return;
            }

            singleton = this;
            DontDestroyOnLoad(gameObject);
        }

        public void ProgressScene(GameState gameState)
        {
            GameHandler.gameState = gameState;
            switch (gameState)
            {
                case GameState.NETWORK_SERVER:
                    manager.StartHost();
                    break;

                case GameState.NETWORK_CLIENT:
                    manager.networkAddress = targetIp;
                    manager.StartClient();
                    break;

                case GameState.LOCAL:
                    SceneManager.LoadScene(SCENE_LOCAL);
                    break;

                case GameState.IDLE:
                default:
                    SceneManager.LoadScene(SCENE_IDLE);
                    return;
            }
        }
    }

    public enum GameState
    {
        IDLE,
        NETWORK_SERVER,
        NETWORK_CLIENT,
        LOCAL
    }
}
