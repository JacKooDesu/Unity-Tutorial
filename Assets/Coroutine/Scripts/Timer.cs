using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoroutineUtility
{
    public class Timer
    {
        static int globalId = 0;
        int id;
        public string ID
        {
            get => $"timer-{id}";
        }
        bool hasRun = false;

        System.Action begin = () => { };
        System.Action<float> update = (f) => { };
        System.Action complete = () => { };

        public Timer(float time, System.Action complete, bool autoRun = true)
        {
            id = globalId;
            globalId++;

            this.complete = complete;

            if (autoRun)
                Manager.Singleton.Add(Run(time), $"timer-{id}");
        }

        public Timer(float time, System.Action begin, System.Action<float> update, System.Action complete, bool autoRun = true)
        {
            id = globalId;
            globalId++;

            this.begin = begin;
            this.update = update;
            this.complete = complete;

            if (autoRun)
                Manager.Singleton.Add(Run(time), $"timer-{id}");
        }

        IEnumerator Run(float time)
        {
            if (hasRun)
                yield break;

            hasRun = true;

            float t = 0;
            begin.Invoke();

            while (t < time)
            {
                update.Invoke(t);
                t += Time.deltaTime;
                yield return null;
            }

            complete.Invoke();
        }

        public void Stop()
        {
            Manager.Singleton.Stop($"timer-{id}");
        }
    }
}
