using HoloToolkit.Unity.InputModule;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using System;

public class VoiceRecognizer : MonoBehaviour {

    // https://docs.microsoft.com/en-us/windows/mixed-reality/voice-input-in-unity

    private KeywordRecognizer kwr;
    private Dictionary<string, Action> keywords = new Dictionary<string, Action>();
    private List<GameObject> nodes;

	// Use this for initialization
	void Start () {
        nodes = new List<GameObject>();
        
        keywords.Add("freeze graph", () =>
        {
            Debug.Log("Freeze graph recognized");
            GameObject[] upper = GameObject.FindGameObjectsWithTag("upper");
            GameObject[] lower = GameObject.FindGameObjectsWithTag("lower");
            foreach (GameObject go in upper)
            {
                go.GetComponent<NodePhysX>().enabled = false;
                go.GetComponent<Rigidbody>().isKinematic = true;
            }
            foreach (GameObject go in lower)
            {
                go.GetComponent<NodePhysX>().enabled = false;
                go.GetComponent<Rigidbody>().isKinematic = true;
            }
        });

        keywords.Add("stop freeze", () =>
        {
            Debug.Log("Freeze graph recognized");
            GameObject[] upper = GameObject.FindGameObjectsWithTag("upper");
            GameObject[] lower = GameObject.FindGameObjectsWithTag("lower");
            foreach (GameObject go in upper)
            {
                go.GetComponent<NodePhysX>().enabled = true;
                go.GetComponent<Rigidbody>().isKinematic = false;
            }
            foreach (GameObject go in lower)
            {
                go.GetComponent<NodePhysX>().enabled = true;
                go.GetComponent<Rigidbody>().isKinematic = false;
            }
        });

        keywords.Add("enable place", () =>
        {
            Debug.Log("Tapping to place recognized");
            GameObject[] upper = GameObject.FindGameObjectsWithTag("upper");
            GameObject[] lower = GameObject.FindGameObjectsWithTag("lower");
            foreach (GameObject go in upper)
            {
                go.GetComponent<NodePhysX>().enabled = false;
                go.GetComponent<Interactible>().enabled = false;
                go.GetComponent<TapToPlace>().enabled = true;
                go.GetComponent<Rigidbody>().isKinematic = true;
            }
            foreach (GameObject go in lower)
            {
                go.GetComponent<NodePhysX>().enabled = false;
                go.GetComponent<Interactible>().enabled = false;
                go.GetComponent<TapToPlace>().enabled = true;
                go.GetComponent<Rigidbody>().isKinematic = true;
            }
        });

        keywords.Add("stop placing", () =>
        {
            Debug.Log("Tapping to place recognized");
            GameObject[] upper = GameObject.FindGameObjectsWithTag("upper");
            GameObject[] lower = GameObject.FindGameObjectsWithTag("lower");
            foreach (GameObject go in upper)
            {
                go.GetComponent<NodePhysX>().enabled = true;
                go.GetComponent<Interactible>().enabled = true;
                go.GetComponent<TapToPlace>().enabled = false;
                go.GetComponent<Rigidbody>().isKinematic = false;
            }
            foreach (GameObject go in lower)
            {
                go.GetComponent<NodePhysX>().enabled = true;
                go.GetComponent<Interactible>().enabled = true;
                go.GetComponent<TapToPlace>().enabled = false;
                go.GetComponent<Rigidbody>().isKinematic = false;
            }
        });

        kwr = new KeywordRecognizer(keywords.Keys.ToArray());

        kwr.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;

        kwr.Start();
	}

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Action keywordAction;
        if(keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }

    }
}
