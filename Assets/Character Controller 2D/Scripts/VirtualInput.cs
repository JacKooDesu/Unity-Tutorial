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

        public bool changeState = false;
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
        // FOR TEST
        // var mousePos = (Vector2)Input.mousePosition - (offset / 2);
        // // print(mousePos);
        // foreach (var key in virtualKeys)
        // {
        //     var rect = key.trigger.rect;
        //     rect.center = key.trigger.localPosition;

        //     print(rect);

        //     if (rect.Contains(mousePos))
        //         print(key.name);
        // }
    }

    private void FixedUpdate()
    {
        foreach (var touch in Input.touches)
        {
            var touchPos = touch.position - (offset / 2);
            // print(touch.position);
            foreach (var key in virtualKeys)
            {
                if (key.lastState != key.currentState)
                    key.changeState = true;
                else
                    key.changeState = false;

                key.lastState = key.currentState;
            }
        }
    }
}
