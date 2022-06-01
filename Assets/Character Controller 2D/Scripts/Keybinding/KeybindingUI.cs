using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeybindingUI : MonoBehaviour
{
    public Keybinding keybinding;
    public GameObject prefabButton;
    public Button toggle;

    [SerializeField] List<Button> buttons = new List<Button>();

    [ContextMenu("生成UI")]
    void InitUi()
    {
        for (int i = transform.childCount - 1; i >= 0; --i)
            DestroyImmediate(transform.GetChild(i).gameObject);

        buttons = new List<Button>();
        foreach (var k in keybinding.keySettings)
        {
            var go = Instantiate(prefabButton, transform);
            go.name = k.name;

            Text text;
            if ((text = prefabButton.GetComponentInChildren<Text>()) != null)
            {
                text.text = $"{k.name} : {k.keyCode.ToString()}";
            }

            buttons.Add(go.GetComponent<Button>());
        }
    }

    public void UpdateButton()
    {
        for (int i = 0; i < buttons.Count; ++i)
        {
            var setting = keybinding.keySettings[i];
            buttons[i].GetComponentInChildren<Text>().text =
                $"{setting.name} : {setting.keyCode.ToString()}";
        }

    }

    void Start()
    {
        for (int i = 0; i < buttons.Count; ++i)
        {
            int x = i;
            buttons[i].onClick.AddListener(() => StartCoroutine(keybinding.Binding(x)));
        }

        keybinding.onBindKey += () => UpdateButton();
        toggle.onClick.AddListener(() => gameObject.SetActive(!gameObject.activeInHierarchy));
        gameObject.SetActive(!gameObject.activeInHierarchy);

        UpdateButton();
    }
}
