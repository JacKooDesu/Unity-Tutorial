using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualInput : MonoBehaviour
{
    [System.Serializable]
    public class VirtualKey
    {
        public string name;
        public RectTransform trigger;

        public enum State
        {
            Released,
            Pressing
        }

        public State currentState = State.Released;
        public State lastState = State.Released;

        public bool isPressing => currentState == State.Pressing;
        public bool isReleased => currentState == State.Released;

        public bool changeState = false;

        public void UpdateState()
        {
            if (lastState != currentState)
                changeState = true;
            else
                changeState = false;

            lastState = currentState;
        }
    }

    public List<VirtualKey> virtualKeys = new List<VirtualKey>();

    Vector2Int offset;

    private void OnEnable()
    {
        // foreach (var key in virtualKeys)
        // {
        //     var pressEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        //     pressEntry.callback.AddListener((e) => { key.currentState = VirtualKey.State.Pressing; });

        //     var releaseEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        //     releaseEntry.callback.AddListener((e) => { key.currentState = VirtualKey.State.Released; });

        //     // var entryEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        //     // entryEntry.callback.AddListener((e) => { if(Input.tou) key.currentState = VirtualKey.State.Released; });

        //     var exitEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        //     exitEntry.callback.AddListener((e) => { key.currentState = VirtualKey.State.Released; });

        //     key.trigger.triggers.Add(pressEntry);
        //     key.trigger.triggers.Add(releaseEntry);
        //     key.trigger.triggers.Add(exitEntry);
        // }

        offset = new Vector2Int(Screen.width, Screen.height);
    }

    private void Update()
    {
        // MouseDebug();

        print(Input.touchCount);
        // foreach(var t in Input.touches)
        //     print(t.position);
    }

    void MouseDebug()
    {
        // FOR TEST
        var mousePos = (Vector2)Input.mousePosition - (offset / 2);
        // print(mousePos);
        foreach (var key in virtualKeys)
        {
            var rect = key.trigger.rect;
            rect.center = key.trigger.localPosition;

            if (rect.Contains(mousePos))
            {
                key.currentState = VirtualKey.State.Pressing;
            }
            else
            {
                key.currentState = VirtualKey.State.Released;
            }

            key.UpdateState();
        }
    }

    private void FixedUpdate()
    {
        foreach (var key in virtualKeys)
        {
            if (Input.touchCount == 0)
            {
                key.currentState = VirtualKey.State.Released;
                key.UpdateState();
                continue;
            }

            var rect = key.trigger.rect;
            rect.center = key.trigger.localPosition;

            foreach (var touch in Input.touches)
            {
                var touchPos = touch.position - (offset / 2);
                if (rect.Contains(touchPos))
                {
                    key.currentState = VirtualKey.State.Pressing;
                    break;
                }
                else
                {
                    key.currentState = VirtualKey.State.Released;
                }
            }

            key.UpdateState();
        }
    }
}
