using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityCoroutine : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Coroutine());
    }

    IEnumerator Coroutine()
    {
        yield return new WaitUntil(() => true);
        yield return null;
    }
}
