using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WiggleTutorial
{
    public class Wiggle : MonoBehaviour
    {
        public float xScale, yScale;
        public float speed;

        Vector3 origin;

        void Start()
        {
            origin = transform.position;    // 取得物件位置    
        }

        // Update is called once per frame
        void Update()
        {
            float x, y;
            x = Mathf.PerlinNoise(Time.time * speed, 0) - .5f;
            y = Mathf.PerlinNoise(0, Time.time * speed) - .5f;

            Vector3 value = new Vector3(x * xScale, y * yScale, 0);

            transform.position = origin + value;
        }
    }

}
