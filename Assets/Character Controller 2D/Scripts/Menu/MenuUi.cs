using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using C2DGame.GameSystem;

public class MenuUi : MonoBehaviour
{
    public Button serverBtn, clientBtn, localBtn;
    public InputField ipInputField;
    public InputField nameInputField;

    private void Awake()
    {
        GameHandler.gameState = GameState.IDLE;

        ipInputField.text = GameHandler.targetIp;
    }

    public void ToggleButtons(string ip)
    {
        clientBtn.interactable = !string.IsNullOrWhiteSpace(ip);
    }

    private void Start()
    {
        serverBtn.onClick.AddListener(() => GameHandler.Singleton.ProgressScene(GameState.NETWORK_SERVER));

        clientBtn.onClick.AddListener(() => GameHandler.Singleton.ProgressScene(GameState.NETWORK_CLIENT));

        localBtn.onClick.AddListener(() => GameHandler.Singleton.ProgressScene(GameState.LOCAL));

        ipInputField.onValueChanged.AddListener((ip) => GameHandler.targetIp = ip);

        nameInputField.onValueChanged.AddListener((name) => GameHandler.PlayerName = name);

        nameInputField.text = GameHandler.PlayerName;
    }
}
