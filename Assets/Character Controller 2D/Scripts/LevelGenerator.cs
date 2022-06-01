using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jackoo.Utils;

public class LevelGenerator : MonoBehaviour
{
    public int stageCount;
    [SerializeField] StageSetting stageSetting;
    List<Stage> stgs = new List<Stage>();
    [SerializeField] List<Rect> rects = new List<Rect>();    // 計算是否重疊

    public bool loadMode;
    public static string targetLevelName;

    private void Start()
    {
        if (!loadMode)
            Generate();
            //StartCoroutine(TestGenerate());
        else
            LoadLevel(targetLevelName);
    }

    [ContextMenu("測試")]
    public void Generate()
    {
        var first = Instantiate(stageSetting.connector).GetComponent<Connector>();
        first.Setup(-1, Vector2.zero, Direction.NONE, Direction.RIGHT);
        stgs.Add(first);
        rects.Add(first.Bounds2Rect());

        int iter = 0;
        int maxTry = 10;
        while (iter < stageCount && maxTry >= 0)
        {
            if (Connect())
            {
                iter++;
            }


            if (maxTry == 0)
            {
                Destroy(stgs[stgs.Count - 1].gameObject);
                stgs.RemoveAt(stgs.Count - 1);
                rects.RemoveAt(rects.Count - 1);
                InitConnector();
                maxTry = 10;
            }
            maxTry--;
        }

        var lastDir = stgs[stgs.Count - 1].outDir;
        var last = Instantiate(stageSetting.connector).GetComponent<Connector>();
        last.Setup(-1, stgs[stgs.Count - 1].exit.position, DirectionUtils.DirectionInverse(lastDir), Direction.NONE);
        stgs.Add(last);
    }

    public bool Connect()
    {
        var lastDir = stgs[stgs.Count - 1].outDir;
        var pos = stgs[stgs.Count - 1].exit.position;
        Stage prefab;
        int rand = 0;
        if (lastDir == Direction.RIGHT || lastDir == Direction.LEFT)
        {
            rand = Random.Range(0, stageSetting.horizontalStgs.Count);
            prefab = stageSetting.horizontalStgs[rand];
        }
        else
        {
            rand = Random.Range(0, stageSetting.verticalStgs.Count);
            prefab = stageSetting.verticalStgs[rand];
        }

        var stg = Instantiate(prefab).GetComponent<Stage>();

        stg.Setup(rand, pos, DirectionUtils.DirectionInverse(lastDir));
        foreach (var r in rects)
        {
            if (stg.Bounds2Rect().Overlaps(r))
            {
                print("Overalp!");
                Destroy(stg.gameObject);
                return false;
            }
        }

        stgs.Add(stg);
        rects.Add(stg.Bounds2Rect());

        InitConnector();

        return true;
    }

    void InitConnector()
    {
        var lastStg = stgs[stgs.Count - 1];
        var nextDir = (Direction)Random.Range(0, 4);
        while (nextDir == DirectionUtils.DirectionInverse(lastStg.outDir))
            nextDir = (Direction)Random.Range(0, 4);
        var cnt = Instantiate(stageSetting.connector).GetComponent<Connector>();
        cnt.Setup(-1, lastStg.exit.position, DirectionUtils.DirectionInverse(lastStg.outDir), nextDir);

        foreach (var r in rects)
        {
            if (cnt.Bounds2Rect().Overlaps(r))
            {
                Destroy(cnt.gameObject);
                Destroy(stgs[stgs.Count - 1].gameObject);
                stgs.RemoveAt(stgs.Count - 1);
                rects.RemoveAt(rects.Count - 1);
                return;
            }
        }

        stgs.Add(cnt);
        rects.Add(cnt.Bounds2Rect());
    }

    public IEnumerator TestGenerate()
    {
        float interval = 1f;

        var first = Instantiate(stageSetting.connector).GetComponent<Connector>();
        first.Setup(-1, Vector2.zero, Direction.NONE, Direction.RIGHT);
        stgs.Add(first);
        rects.Add(first.Bounds2Rect());

        int iter = 0;
        int maxTry = 10;
        while (iter < stageCount && maxTry >= 0)
        {
            if (Connect())
            {
                iter++;
                yield return new WaitForSeconds(interval);
            }


            if (maxTry == 0)
            {
                Destroy(stgs[stgs.Count - 1].gameObject);
                stgs.RemoveAt(stgs.Count - 1);
                rects.RemoveAt(rects.Count - 1);
                InitConnector();
                maxTry = 10;
            }
            maxTry--;
        }

        var lastDir = stgs[stgs.Count - 1].outDir;
        var last = Instantiate(stageSetting.connector).GetComponent<Connector>();
        last.Setup(-1, stgs[stgs.Count - 1].exit.position, DirectionUtils.DirectionInverse(lastDir), Direction.NONE);
        stgs.Add(last);

        yield break;
    }

    public void SaveLevel()
    {
        if (loadMode)
            return;

        FileManagerTutorial.FileManager<LevelData>.Save(
            $"Lv-{System.DateTime.Now.ToString("MM-dd-yyyy-HH-mm")}",
            LevelUtil.Stages2Data(stageSetting, stgs),
            "Save/Levels");
    }

    public void LoadLevel(string levelName, string version = "1.0")
    {
        var data = new LevelData();
        FileManagerTutorial.FileManager<LevelData>.Load("Save/Levels", levelName, data);
        List<Stage> tempStages = new List<Stage>();

        foreach (var stgData in data.stageDatas)
        {
            Stage prefab;
            switch (stgData.stgType)
            {
                case Stage.StageType.Horizontal:
                    prefab = stageSetting.horizontalStgs[stgData.index];
                    break;

                case Stage.StageType.Vertical:
                    prefab = stageSetting.verticalStgs[stgData.index];
                    break;

                case Stage.StageType.Connector:
                    prefab = stageSetting.connector;
                    break;

                default:
                    return;
            }
            var stg = Instantiate(prefab);
            stg.stgTypeIndex = stgData.index;
            stg.inDir = stgData.inDir;
            stg.outDir = stgData.outDir;
            stg.inverse = stgData.inverse;
            tempStages.Add(stg);
        }

        var first = (tempStages[0] as Connector);
        first.Setup(-1, Vector2.zero, first.inDir, first.outDir);
        stgs.Add(first);
        for (int i = 1; i < tempStages.Count; ++i)
        {
            var stg = tempStages[i];
            var lastStg = tempStages[stgs.Count - 1];
            if (stg.stgType == Stage.StageType.Connector)
            {
                var cnt = (stg as Connector);
                // cnt.Setup(-1, lastStg.exit.position, cnt.inDir, cnt.outDir);
                cnt.Setup(-1, lastStg.exit.position, cnt.inDir, cnt.outDir);

                stgs.Add(cnt);
            }
            else
            {
                stg.Setup(stg.stgTypeIndex, lastStg.exit.position, DirectionUtils.DirectionInverse(lastStg.outDir), stg.inverse);
                stgs.Add(stg);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var rect in rects)
        {
            Gizmos.color = new Color(1, 0, 1, .2f);
            Gizmos.DrawCube(rect.center, rect.size);
        }
    }
}

public static class DirectionUtils
{
    public static Direction DirectionInverse(Direction dir)
    {
        int tmp = (int)dir;

        tmp += 2;
        tmp -= (tmp > 3 ? 4 : 0);

        return (Direction)tmp;
    }
}

public enum Direction
{
    NONE = -1,
    UP,
    RIGHT,
    DOWN,
    LEFT
}