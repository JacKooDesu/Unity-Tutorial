using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptObjectTutorial
{
    public class Enemy : MonoBehaviour
    {
        public EnemySetting setting;
        // public float speed;
        // public Color color;

        Vector2 originVel;
        Vector2 velocity;

        // curve settings
        float length;
        float offset;
        int step = 100;
        float max, min;

        void Start()
        {
            originVel = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            velocity = originVel;

            SetupCurveValues();
            Visualize();
        }

        public void Visualize()
        {
            transform.localScale = Vector3.one * setting.size;
            GetComponent<SpriteRenderer>().color = setting.color;
        }

        void SetupCurveValues()
        {
            var curve = setting.curve;
            length = curve.keys[curve.length - 1].time - curve.keys[0].time;
            var interval = length / (float)step;
            float temp = 0;
            max = float.MinValue;
            min = float.MaxValue;
            for (int i = 0; i < step; ++i)
            {
                if (curve.Evaluate(temp) < min)
                    min = curve.Evaluate(temp);

                if (curve.Evaluate(temp) > max)
                    max = curve.Evaluate(temp);
                temp += interval;
            }
        }

        void CheckBounce()
        {
            var currentPos = Camera.main.WorldToScreenPoint(transform.position);

            if (currentPos.x >= Screen.width || currentPos.x <= 0 ||
                currentPos.y >= Screen.height || currentPos.y <= 0)
            {
                if (currentPos.x >= Screen.width)
                    velocity.x = -Mathf.Abs(velocity.x);

                if (currentPos.x <= 0f)
                    velocity.x = Mathf.Abs(velocity.x);

                if (currentPos.y >= Screen.height)
                    velocity.y = -Mathf.Abs(velocity.y);

                if (currentPos.y <= 0f)
                    velocity.y = Mathf.Abs(velocity.y);

                print(currentPos);
            }
        }

        void Update()
        {
            CheckBounce();
            var evaluate = setting.curve.Evaluate(offset % length) / (max == min ? 1 : max - min);
            var wave = Vector2.Perpendicular(velocity) * evaluate * setting.curveScale;
            var v = (Vector3)((velocity + wave) * setting.speed * Time.deltaTime);
            offset += Time.deltaTime;
            transform.position += v;
        }
    }
}