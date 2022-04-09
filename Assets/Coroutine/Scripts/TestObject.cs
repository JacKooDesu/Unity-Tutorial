using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoroutineUtility
{
    public class TestObject : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var t = new CoroutineUtility.Timer(3f, () => Test());
            t.Stop();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void Test()
        {
            print($"{transform.GetInstanceID()} test");
        }
    }

}
