using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PrintMessage : MonoBehaviour
{
    public string address = "127.0.0.1";
    private UnityWebRequest www;

    void Start()
    {
        StartCoroutine(GetText());
    }

    private IEnumerator GetText()
    {
        // https://docs.unity3d.com/ScriptReference/WWW.html

        www = UnityWebRequest.Get(address);


        yield return www.SendWebRequest();
        if (www.isHttpError || www.isNetworkError)
        {
            print("error");
            print(www.error);
        }
        else
        {
            print(www.downloadHandler.data);
            print(www.downloadHandler.text);
        }

    }
    void Update()
    {
        print(www.downloadProgress);
    }

}