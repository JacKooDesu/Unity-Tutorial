using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageSetting", menuName = "Unity-Tutorial/StageSetting", order = 0)]
public class StageSetting : ScriptableObject
{
    public Connector connector;
    public List<Stage> horizontalStgs = new List<Stage>();
    public List<Stage> verticalStgs = new List<Stage>();
}
