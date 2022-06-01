using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keybinding : MonoBehaviour
{
    public List<KeySetting> keySettings = new List<KeySetting>();

    [SerializeField] KeybindingUI ui;

    bool isBinding;

    public System.Action onBindKey;
    public IEnumerator Binding(int index)
    {
        if (isBinding)
            yield break;

        float originTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        while (true)
        {
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(vKey))
                {
                    keySettings[index].keyCode = vKey;
                    isBinding = false;
                    ui.UpdateButton();

                    if (onBindKey != null)
                    {
                        onBindKey.Invoke();
                        Time.timeScale = originTimeScale;
                    }
                    yield break;
                }
            }
            yield return null;
        }
    }

    private void Start()
    {
        Keybinding tempK = new Keybinding();
        FileManagerTutorial.FileManager<Keybinding>.Load("Save", "key-mapping", tempK);
        this.keySettings = tempK.keySettings;
        onBindKey += () =>
        {
            FileManagerTutorial.FileManager<Keybinding>.Save("key-mapping", this, "Save");
        };

        if (onBindKey != null)
            onBindKey.Invoke();
    }
}

[System.Serializable]
public class KeySetting
{
    public string name;
    public KeyCode keyCode;
}
