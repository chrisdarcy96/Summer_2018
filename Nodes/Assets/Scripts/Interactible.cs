using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

//TODO: Potentially may want to rename this.

// Kudos to a stare-select explanation done for the Google Cardboard by this StackOverflow user:
// https://stackoverflow.com/questions/34384382/use-gaze-input-duration-to-select-ui-text-in-google-cardboard/40842739#40842739
public class Interactible : MonoBehaviour, IFocusable {

    public Shader glow;
    public bool beingLookedAt = false;
    public bool isSelected = false;
    public float stareTriggerDuration = 3f;
    private float stareTimer = 0f;
    private GameObject GameController = GameObject.Find("GameController");
    
    

    public void OnFocusEnter()
    {
        GetComponent<Renderer>().material.shader = glow;
        beingLookedAt = true;
    }



    public void OnFocusExit()
    {
        GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
    }

    // Use this for initialization
    void Start () {

}
	
	// Update is called once per frame
	void Update () {
		if (beingLookedAt)
        {
            stareTimer += Time.deltaTime;
            if(stareTimer >= stareTriggerDuration)
            {
                isSelected = true;
                SelectionManager.HandleSelection(this.gameObject);
            }
        }
	}
}
