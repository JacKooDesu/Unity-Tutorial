using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;

public class DiscordRichPresenceHandler : MonoBehaviour
{
    Discord.Discord discord;
    Discord.Activity activity;
    public string state = "狀態";
    public string details = "遊戲內容";
    public string iconName;

    void Start()
    {
        discord = new Discord.Discord(980733539119153162, (System.UInt64)Discord.CreateFlags.Default);
        var activityManager = discord.GetActivityManager();
        activity = new Discord.Activity
        {
            State = state,
            Details = details,
            Assets = {
                LargeImage = iconName
            }
        };

        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res == Discord.Result.Ok)
                Debug.Log("Discord 狀態載入成功");
            else
                Debug.LogError($"Discord 狀態載入失敗 / {res}");
        });
    }

    void Update()
    {
        discord.RunCallbacks();
    }
}
