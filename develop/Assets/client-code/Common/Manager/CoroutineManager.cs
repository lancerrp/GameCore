using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviourSingleton<CoroutineManager>
{
    public void StartCorout(IEnumerator routine)
    {
        StartCoroutine(routine);
    }

    public void StopCorout(IEnumerator routine)
    {
        StopCoroutine(routine);
    }

    public void StopAll()
    {
        StopAllCoroutines();
    }
}