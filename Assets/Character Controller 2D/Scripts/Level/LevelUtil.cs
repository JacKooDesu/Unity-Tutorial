using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelUtil
{
    // 存檔用
    public static LevelData Stages2Data(StageSetting setting, List<Stage> stages, string version = "1.0")
    {
        LevelData levelData = new LevelData();
        levelData.version = version;
        foreach (var stg in stages)
        {
            var data = new LevelData.StageData
            {
                stgType = stg.stgType,
                index = stg.stgTypeIndex,
                inDir = stg.inDir,
                outDir = stg.outDir,
                inverse = stg.inverse
            };

            levelData.stageDatas.Add(data);
        }

        return levelData;
    }
}
