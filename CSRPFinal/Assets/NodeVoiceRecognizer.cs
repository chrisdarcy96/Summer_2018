using HoloToolkit.Unity.InputModule;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using System;

public class NodeVoiceRecognizer : MonoBehaviour
{

    // https://docs.microsoft.com/en-us/windows/mixed-reality/voice-input-in-unity

    private KeywordRecognizer kwr;
    private Dictionary<string, Action> keywords = new Dictionary<string, Action>();

    // Use this for initialization
    void Start()
    {
        keywords.Add("tap to place", () =>
        {
            this.gameObject.GetComponent<Interactible>().enabled = !this.gameObject.GetComponent<Interactible>().enabled;
            this.gameObject.GetComponent<TapToPlace>().enabled = !this.gameObject.GetComponent<TapToPlace>().enabled;
        });

        kwr = new KeywordRecognizer(keywords.Keys.ToArray());

        kwr.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;

        kwr.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}
