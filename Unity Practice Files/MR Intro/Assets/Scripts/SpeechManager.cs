using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows.Speech;
using System;

public class SpeechManager : MonoBehaviour {

    KeywordRecognizer keyRec = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

	// Use this for initialization
	void Start () {

        // adds keywords and system actions into dictionary ----
        keywords.Add("Reset world", () =>
        {
            this.BroadcastMessage("OnReset"); // OnReset called on every object
        });

        keywords.Add("Drop Sphere", () =>
        {
            GameObject focusObject = GazeGestureManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                focusObject.SendMessage("OnDrop", SendMessageOptions.DontRequireReceiver); // call OnDrop on focusedObject
            }
        });
        // end block ----------------------------

        // tell KeywordRecognizer about keywords
        keyRec = new KeywordRecognizer(keywords.Keys.ToArray());

        // create callback and start recognizing
        keyRec.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keyRec.Start();
	}

    // helper method calls the action when keyword is said
    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keyAction;
        if(keywords.TryGetValue(args.text, out keyAction))
        {
            keyAction.Invoke();
        }
    }

}
