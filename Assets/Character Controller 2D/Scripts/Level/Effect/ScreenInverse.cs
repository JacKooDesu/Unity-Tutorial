using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScreenInverse : MonoBehaviour
{
    public bool isOn;
    public Material testMat;
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (!isOn)
        {
            Graphics.Blit(src, dest, new Vector2(1, 1), Vector2.zero);
        }
        else
        {
            Graphics.Blit(src, dest,testMat);
        }
    }
}
