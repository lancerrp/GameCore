using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        GameConfigManager.instance.OnInit();

        text = GetComponentInChildren<TextMeshProUGUI>();
        button.onClick.AddListener(() => 
        {
            var conf = GameConfigManager.instance.LoadSheetConfig<Config.TestSheet>(1);
            text.text = conf.name;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("≤‚ ‘")]
    private void T1()
    {
    }

    [ContextMenu("≤‚ ‘2")]
    private void T2()
    {
        var conf = GameConfigManager.instance.LoadXmlConfig<Config.TestSheet[]>("test_sheet");
        text.text = conf.Length.ToString();
    }
}