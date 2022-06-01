using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public class LevelPlayData
    {
        public string lvName;
        public float time;
    }
    
    public List<LevelPlayData> levelPlayDatas = new List<LevelPlayData>();
}
