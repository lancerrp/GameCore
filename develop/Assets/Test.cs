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
        var conf = GameConfigManager.instance.LoadSheetConfig<Config.TestSheet>(1);
    }
}