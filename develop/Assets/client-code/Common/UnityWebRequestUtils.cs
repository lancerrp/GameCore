using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UnityWebRequestUtils
{
    public static IEnumerator LoadStream(string path, Action<bool, Stream> callBack) 
    {
        using (var request = UnityEngine.Networking.UnityWebRequest.Get(path))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Stream steam = new MemoryStream(request.downloadHandler.data);
                callBack?.Invoke(true, steam);
            }
            else
            {
                Helper.LogError(request.error);
                callBack?.Invoke(false, null);
            }
        }
    }

    //Õ¨≤Ωº”‘ÿ
    public static void LoadStreamSync(string path, Action<bool, Stream> callBack)
    {
        using (var request = UnityEngine.Networking.UnityWebRequest.Get(path))
        {
            request.SendWebRequest();
            while (!request.isDone) 
            {
            }
            if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Stream steam = new MemoryStream(request.downloadHandler.data);
                callBack?.Invoke(true, steam);
            }
            else
            {
                Helper.LogError(request.error);
                callBack?.Invoke(false, null);
            }
        }
    }
}
