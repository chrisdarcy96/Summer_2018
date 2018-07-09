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
        GameObject[] upper = GameObject.FindGameObjectsWithTag("upper");
        GameObject[] lower = GameObject.FindGameObjectsWithTag("lower");
        nodes.AddRange(upper);
        nodes.AddRange(lower);

        keywords.Add("freeze graph", () =>
        {
            
            foreach(GameObject go in nodes)
            {
                go.GetComponent<NodePhysX>().enabled = !go.GetComponent<NodePhysX>().enabled;
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
