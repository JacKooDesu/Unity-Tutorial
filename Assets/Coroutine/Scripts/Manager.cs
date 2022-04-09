using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoroutineUtility
{
    public class Manager : MonoBehaviour
    {
        static Manager singleton = null;

        public static Manager Singleton
        {
            get
            {
                if (singleton != null)
                    return singleton;

                singleton = FindObjectOfType(typeof(Manager)) as Manager;

                if (singleton == null)
                {
                    GameObject g = new GameObject("CoroutineUtility");
                    singleton = g.AddComponent<Manager>();
                }

                return singleton;
            }
        }

        private void OnEnable()
        {
            DontDestroyOnLoad(gameObject);
        }

        public static Dictionary<string, List<Coroutine>> coroutines = new Dictionary<string, List<Coroutine>>();

        public void Add(IEnumerator coroutine, string id)
        {
            var c = StartCoroutine(coroutine);
            if (coroutines.ContainsKey(id))
                coroutines[id].Add(c);
            else
                coroutines.Add(id, new List<Coroutine>() { c });
        }

        public void Stop(string id)
        {
            if (!coroutines.ContainsKey(id))
            {
                Debug.LogWarning($"{id} 尚未登記");
                return;
            }

            foreach (var c in coroutines[id])
                if (c != null) StopCoroutine(c);

            coroutines.Remove(id);

            Debug.Log($"{id} 已被停止");
        }
    }

}
