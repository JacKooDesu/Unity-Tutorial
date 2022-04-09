using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptObjectTutorial
{
    [CreateAssetMenu(fileName = "new Enemy Setting", menuName = "ScriptObject Tutorial/EnemySetting", order = 0)]
    public class EnemySetting : ScriptableObject
    {
        public float speed;

        public Color color;
        public AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0f), new Keyframe(1, 0f));
        public float curveScale = 5f;
    }

}