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

	// Use this for initialization
	void Start () {
        keywords.Add("tap to place", () =>
        {
            // action that should happen
            this.GetComponent<TapToPlace>().enabled = !this.GetComponent<TapToPlace>().enabled;
        });

        keywords.Add("freeze graph", () =>
        {
            GameObject[] upper = GameObject.FindGameObjectsWithTag("upper");
            GameObject[] lower = GameObject.FindGameObjectsWithTag("lower");
            foreach(GameObject go in upper)
            {
                if (go.GetComponent<NodePhysX>().isActiveAndEnabled)
                {
                    go.GetComponent<NodePhysX>().enabled = false;
                }
            }
            foreach (GameObject go in lower)
            {
                if (go.GetComponent<NodePhysX>().isActiveAndEnabled)
                {
                    go.GetComponent<NodePhysX>().enabled = false;
                }
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

    // Update is called once per frame
    void Update () {
		
	}
}
