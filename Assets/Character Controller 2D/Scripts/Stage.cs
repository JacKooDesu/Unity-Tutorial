using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public Direction inDir, outDir;
    public Vector2 stageSize;
    public Transform entrance, exit;
    public Bounds bounds;
    [HideInInspector] public int stgTypeIndex;  // 種類index，用於存檔

    public enum StageType
    {
        Horizontal,
        Vertical,
        Connector
    }

    public StageType stgType;
    [HideInInspector] public bool inverse = false;

    public void Setup(int index, Vector2 pos, Direction lastDir)
    {
        this.stgTypeIndex = index;
        if (lastDir != inDir)
        {
            // 可能須改寫
            switch (stgType)
            {
                case StageType.Horizontal:
                    transform.localScale = new Vector3(-1, 1, 1);
                    break;

                case StageType.Vertical:
                    transform.localScale = new Vector3(1, -1, 1);
                    break;
            }
            var tmpDir = inDir;
            inDir = outDir;
            outDir = tmpDir;

            inverse = true;
        }
        PositionOffset(pos);
        SetBounds();
    }

    public void PositionOffset(Vector2 pos)
    {
        Vector2 offset = entrance.position - transform.position;
        transform.position = pos - offset;
    }

    [ContextMenu("計算大小")]
    protected void SetBounds()
    {
        bounds = CalculateBounds();
    }

    [ContextMenu("置中物件")]
    protected void CentralObjects()
    {
        foreach (Transform t in transform)
        {
            t.localPosition = bounds.center;
        }
    }

    Bounds CalculateBounds()
    {
        var allCol = GetComponentsInChildren<Collider2D>();
        var center = transform.position;
        var bounds = new Bounds();
        bounds.min = allCol[0].bounds.min;
        bounds.max = allCol[0].bounds.max;

        for (int i = 1; i < allCol.Length; ++i)
        {
            var min = bounds.min;
            var max = bounds.max;
            var b = allCol[i].bounds;

            if (b.min.x < min.x)
                min.x = b.min.x;
            if (b.min.y < min.y)
                min.y = b.min.y;
            if (b.min.z < min.z)
                min.z = b.min.z;

            if (b.max.x > max.x)
                max.x = b.max.x;
            if (b.max.y > max.y)
                max.y = b.max.y;
            if (b.max.z > max.z)
                max.z = b.max.z;

            bounds.min = min;
            bounds.max = max;
        }

        return bounds;
    }

    public Rect Bounds2Rect()
    {
        var tmpBound = CalculateBounds();
        if (stgType == StageType.Connector)
            tmpBound.size -= Vector3.one * 2;
        if (stgType == StageType.Horizontal)
            tmpBound.size -= Vector3.right * 2;
        if (stgType == StageType.Vertical)
            tmpBound.size -= Vector3.up * 2;

        var center = Vector2.zero;
        if (inverse)
        {
            center = -tmpBound.center - tmpBound.extents + transform.position;
        }
        else
        {
            center = tmpBound.center - tmpBound.extents + transform.position;
        }

        return new Rect(center, tmpBound.size);
    }

    private void OnDrawGizmosSelected()
    {
        var bounds = CalculateBounds();

        Gizmos.color = new Color(1, 1, 1, .2f);
        Gizmos.DrawCube(bounds.center, bounds.size);
    }

}
