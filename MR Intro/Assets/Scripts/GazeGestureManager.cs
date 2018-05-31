using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class GazeGestureManager : MonoBehaviour {

    public static GazeGestureManager Instance { get; private set; }

    // Represent hologram currently gazed at
    public GameObject FocusedObject { get; private set; }
    GestureRecognizer recognizer;

	// Use this for initialization
	void Awake ()   // called on load
    {
        Instance = this;

        // set up gesture recognizer to detect "Select" gestures
        recognizer = new GestureRecognizer();
        recognizer.Tapped += (args) =>
        {
            // Send an onselect message to the focused object and its ancestors
            if (FocusedObject != null)
            {
                FocusedObject.SendMessageUpwards("OnSelect", SendMessageOptions.DontRequireReceiver);
            }
        };
        recognizer.StartCapturingGestures();
	}
	
	// Update is called once per frame
	void Update () {
        // figure out which object is focused
        GameObject oldFocusObject = FocusedObject;

        // raycast into world based on user's head
        // position and direction
        var headPos = Camera.main.transform.position;
        var gazeDir = Camera.main.transform.forward;

        RaycastHit hitInfo;

        if (Physics.Raycast(headPos, gazeDir, out hitInfo))
        {
            // if raycast hit a hologram
            FocusedObject = hitInfo.collider.gameObject; // use that as focused object
        }
        else
        {
            FocusedObject = null; // if raycast did not hit a hologram, clear focused item
        }

        // If focused object changed this Update
        if(FocusedObject != oldFocusObject)
        {
            // start detecting new gestures again
            recognizer.CancelGestures();
            recognizer.StartCapturingGestures();
        }

    }
}
