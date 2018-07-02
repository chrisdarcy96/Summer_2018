using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

public class PrintMessage : MonoBehaviour
{
    public GameObject prefab;
    public string address = "www.google.com";       // test domain
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
            print(www.downloadHandler.text);
            ConnectionSuccessful();
        }

    }

    private void ConnectionSuccessful()
    {
        prefab = Instantiate(prefab);
    }

    void Update()
    {
        Vector3 pos = 5 * UnityEngine.Random.insideUnitSphere;
        print("Object now at : " + pos);
        prefab.transform.position = pos;
    }

}