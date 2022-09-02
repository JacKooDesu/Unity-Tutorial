using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventHandler : MonoBehaviour
{
    public Block BlockA;
    public Block BlockB;
    public Block BlockC;
    public UnityEvent MoveEvent = new UnityEvent();

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            MoveEvent.Invoke();
        }
    }
}
