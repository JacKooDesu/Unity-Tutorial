using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace FileManagerTutorial
{
    // 使用泛型存檔
    public static class FileManager<T>
    {
        public static void Save(string fileName, T target, string dir)
        {
            var jsonData = JsonUtility.ToJson(target, true);    // 物件序列化json字串
            var filePath = $"{Application.dataPath}/{dir}";     // 取得資料夾路徑

            Directory.CreateDirectory(filePath);        // 確認資料夾
            File.WriteAllText($"{filePath}/{fileName}.sav", jsonData);  // json寫入
        }

        public static T Load(string path, string name)
        {
            var filePath = $"{Application.dataPath}/{path}/{name}.sav";
            var deserializeData = (string)(null);   // json 解譯字串

            try
            {
                deserializeData = File.ReadAllText(filePath);
            }
            catch (System.IO.FileNotFoundException)
            {
                Debug.Log("FileNotFoundException");
                return default(T);
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                Debug.Log("DirectoryNotFoundException");
                return default(T);
            }

            return JsonUtility.FromJson<T>(deserializeData);    // 從json轉換成 T
        }

        // 針對 MonoBehaviour 無法指定到物件上所寫的讀檔功能
        // where 在泛型中指定為 MonoBehaviour
        public static void Load<T1>(string path, string name, T1 target) where T1 : MonoBehaviour
        {
            var filePath = $"{Application.dataPath}/{path}/{name}.sav";
            var deserializeData = (string)(null);

            try
            {
                deserializeData = File.ReadAllText(filePath);
            }
            catch (System.IO.FileNotFoundException)
            {
                Debug.Log("FileNotFoundException");
                return;
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                Debug.Log("DirectoryNotFoundException");
                return;
            }

            JsonUtility.FromJsonOverwrite(deserializeData, target);
        }
    }

}