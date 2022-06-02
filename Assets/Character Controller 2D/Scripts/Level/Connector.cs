using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Connector : Stage
{
    #region 物件綁定
    [SerializeField] Transform rewardParent;

    [Header("邊")]
    [SerializeField] GameObject up;
    [SerializeField] GameObject right;
    [SerializeField] GameObject down;
    [SerializeField] GameObject left;

    [Header("角")]
    [SerializeField] GameObject leftUp;
    [SerializeField] GameObject rightUp;
    [SerializeField] GameObject rightDown;
    [SerializeField] GameObject leftDown;
    GameObject[] objects;

    [SerializeField] Checkpoint checkpoint;

    void InitObjectArray()
    {
        // obslete
        // 0-1-2
        // 7-x-3
        // 6-5-4

        // objects = new GameObject[]{
        //     leftUp,
        //     up,
        //     rightUp,
        //     right,
        //     rightDown,
        //     down,
        //     leftDown,
        //     left
        // };

        // x-0-x
        // 3-x-1
        // x-2-x

        objects = new GameObject[]{
            up,
            right,
            down,
            left
        };
    }

    #endregion

    public void Setup(int index, Vector2 pos, Direction inDir, Direction outDir)
    {
        this.stgTypeIndex = index;
        InitObjectArray();
        foreach (var obj in objects)
            obj.SetActive(true);

        // 頭尾生成
        var dirs = new Direction[] { inDir, outDir };

        foreach (var dir in dirs)
        {
            if (dir == Direction.NONE)
                continue;

            objects[(int)dir].SetActive(false);
        }

        this.inDir = inDir;
        this.outDir = outDir;

        if (inDir != Direction.NONE)
            entrance.position = objects[(int)inDir].transform.position;

        if (outDir != Direction.NONE)
            exit.position = objects[(int)outDir].transform.position;

        PositionOffset(pos);
        SetBounds();

        if (outDir == Direction.NONE)
            checkpoint.onHit += () =>
            {
                FindObjectOfType<LevelGenerator>().SaveLevel();
                SceneManager.LoadScene(0);
            };
    }
}
