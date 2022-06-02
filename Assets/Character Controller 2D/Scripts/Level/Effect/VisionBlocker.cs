using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionBlocker : MonoBehaviour
{
    public LayerMask triggerLayer;
    public bool inactiveWhenEnter = true;

    public GameObject targetMask;

    public List<Collider2D> triggerAreas = new List<Collider2D>();

    private void Update()
    {
        foreach (var area in triggerAreas)
        {
            var bounds = area.bounds;
            var hit = Physics2D.BoxCast(bounds.center, bounds.size, 0, Vector2.up, 0f, triggerLayer);
            if (hit.transform != null)
            {
                targetMask.SetActive(!inactiveWhenEnter);
                return;
            }
        }
        targetMask.SetActive(inactiveWhenEnter);
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var area in triggerAreas)
        {
            if (area == null)
                continue;

            Gizmos.color = new Color(0, 0, 0, .5f);
            Gizmos.DrawCube(area.bounds.center, area.bounds.size);
        }
    }
}
