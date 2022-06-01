using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public ParticleSystem hitBurst;  // 觸碰效果
    public System.Action onHit; // 觸碰事件

    public void Checked()
    {
        hitBurst.Play();
        if (onHit != null)
            onHit.Invoke();
    }
}
