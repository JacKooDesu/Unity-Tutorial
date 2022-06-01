using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string version;
    [System.Serializable]
    public class StageData
    {
        public Stage.StageType stgType;
        public int index;
        public bool inverse;
        public Direction inDir, outDir;
    }

    public List<StageData> stageDatas = new List<StageData>();
}
