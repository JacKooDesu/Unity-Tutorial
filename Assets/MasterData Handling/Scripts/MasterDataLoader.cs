using System.Collections.Generic;
using System.IO;
using System;

using UnityEngine;

namespace MasterDataHandling
{
    public class MasterDataLoader
    {
        public static Dictionary<string, List<T>> Load<T>(string target, string path, params string[] keys) where T : IConvertible
        {
            string dir = $"{Application.dataPath}/{path}/{target}.csv";
            if (!File.Exists(dir))
            {
                Debug.LogError($"Master Data [{target}] not found");
                Debug.LogError($"Error dir : {dir}");
                return null;
            }

            var result = new Dictionary<string, List<T>>();
            var keyIndex = new Dictionary<int, string>();
            using (var reader = new StreamReader(dir))
            {
                var str = "";

                int iter = 0;
                while ((str = reader.ReadLine()) != null)
                {
                    var row = new List<string>(str.Split(','));
                    if (iter == 0)
                    {
                        foreach (var key in keys)
                        {
                            int index = 0;
                            if ((index = row.IndexOf(key)) == -1)
                            {
                                Debug.LogWarning($"Key [{key}] not found in master data!");
                                return null;
                            }
                            else
                            {
                                keyIndex.Add(index, key);
                                result.Add(key, new List<T>());
                            }
                        }
                    }
                    else
                    {
                        if (row.Count - 1 != result.Count)
                        {
                            Debug.LogWarning($"Value length is not match!");
                            return null;
                        }
                        for (int i = 0; i < row.Count; ++i)
                        {
                            if(!keyIndex.ContainsKey(i))
                                continue;
                            var value = row[i];
                            result[keyIndex[i]].Add((T)Convert.ChangeType(value, typeof(T)));
                        }
                    }

                    iter++;
                }

                reader.Close();
            }


            return result;
        }
    }
}

