using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectorUi : MonoBehaviour
{
    const string LEVEL_FOLDER = "Save/Levels";

    public GameObject levelSelectPrefab;
    public Transform contentParent;
    public Transform selectBar;

    public string tempLevelName;

    public Button newGameBtn, loadBtn;

    private void Start()
    {
        LoadFile();

        newGameBtn.onClick.AddListener(() => Play(false));
        loadBtn.onClick.AddListener(() => Play(true));
    }

    void LoadFile()
    {
        try
        {
            DirectoryInfo di = new DirectoryInfo($"{Application.dataPath}/{LEVEL_FOLDER}");
            var files = di.GetFiles("*.sav");
            foreach (var f in files)
            {
                print(f.Name);

                string tempFileName = f.Name.Replace(".sav", "");

                var obj = Instantiate(levelSelectPrefab, contentParent);

                var ev = obj.GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.callback.AddListener((e) =>
                {
                    tempLevelName = tempFileName;
                    selectBar.localPosition = obj.transform.localPosition;
                    selectBar.gameObject.SetActive(true);
                });
                entry.eventID = EventTriggerType.PointerClick;
                ev.triggers.Add(entry);

                obj.GetComponentInChildren<Text>().text = tempFileName;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void Play(bool isLoadMode)
    {
        if (isLoadMode)
        {
            LevelGenerator.targetLevelName = tempLevelName;
            SceneManager.LoadScene(2);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    private void Update()
    {
        if (tempLevelName.Length == 0)
            loadBtn.interactable = false;
        else
            loadBtn.interactable = true;
    }
}
