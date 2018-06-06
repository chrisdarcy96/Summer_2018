using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class NodeGaze : MonoBehaviour, IFocusable {

    public GameObject spawn;
    private int framesViewed = 0;
    private bool isViewing = false;

    // Gaze spawns 3 small spheres at right angles to the focusedObject
    public void OnFocusEnter()
    {
        isViewing = true;
        viewHandler(this.gameObject);
    }

    private void viewHandler(GameObject focusedObject)
    {
        Transform parent = focusedObject.transform;
        print("Transform of parent: " + transform);
    }

    public void OnFocusExit()
    {
        isViewing = false;
        print("Frames focused on this Object: " + framesViewed);
        framesViewed = 0;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isViewing)
        {
            framesViewed += 1;
        }
		
	}
}
