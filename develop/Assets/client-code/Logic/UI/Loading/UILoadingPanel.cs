using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoadingPanel : MonoBehaviour
{
    public static UILoadingPanel instance;

    private TextMeshProUGUI mTips;
    private TextMeshProUGUI mSliderValue;
    private Slider mSlider;

    private bool mFakeFlag = false;
    private float mFakeTime = 0;
    private float mFakeStartValue = 0;
    private float mFakeEndValue = 0;
    private float mTimer = 0;
    private Action mLoadEndCallBack = null;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        mTips = ObjectCommonUtils.GetChildComponent<TextMeshProUGUI>(gameObject, "root/tip");
        mSliderValue = ObjectCommonUtils.GetChildComponent<TextMeshProUGUI>(gameObject, "root/Slider/value");
        mSlider = ObjectCommonUtils.GetChildComponent<Slider>(gameObject, "root/Slider");
        mSlider.onValueChanged.AddListener(OnSliderValueChange);
    }

    private void Update()
    {
        if (!mFakeFlag)
        {
            return;
        }
        mTimer += Time.deltaTime;
        if (mTimer >= mFakeTime) 
        {
            mFakeFlag = false;
            mSlider.value = mFakeEndValue;
            return;
        }
        float fakeValue = Mathf.Lerp(mFakeEndValue, mFakeStartValue, mFakeTime - mTimer);
        mSlider.value = fakeValue;
    }

    public void SetTipsText(int key) 
    {
        string text = StringManager.instance.GetStringValue(key);
        mTips.text = text;
    }

    public void SetTipsText(string text)
    {
        mTips.text = text;
    }

    public void SetLoadValue(float value, float fakeTime = 0, float fakeProgress = 0)
    {
        gameObject.SetActive(true);
        mSlider.value = value;
        mFakeFlag = fakeTime > 0;
        mFakeTime = fakeTime;
        mTimer = 0;
        mFakeStartValue = value;
        mFakeEndValue = value + fakeProgress;
    }

    public void OnLoadEnd(Action callBack, bool close)
    {
        mFakeFlag = false;
        mLoadEndCallBack = callBack;
        mSlider.value = 1;
        if (close) 
        {
            StartCoroutine(DelayClose());
        }
    }

    private void OnSliderValueChange(float value) 
    {
        mSliderValue.text = string.Format("{0}%", (int)(value * 100));
    }

    private IEnumerator DelayClose(float time = 0.2f)
    {
        yield return new WaitForSeconds(time);
        mLoadEndCallBack?.Invoke();
        gameObject.SetActive(false);
    }
}
