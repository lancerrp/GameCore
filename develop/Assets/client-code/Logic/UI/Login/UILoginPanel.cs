using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoginPanel : MonoBehaviour
{
    private Button mStartBtn;

    private void Awake()
    {
        mStartBtn = ObjectCommonUtils.GetChildComponent<Button>(gameObject, "root/start_btn");
        mStartBtn.onClick.AddListener(OnClickStart);
    }

    private void OnClickStart() 
    {
    }
}
