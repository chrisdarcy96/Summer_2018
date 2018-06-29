using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

//TODO: Potentially may want to rename this.
 
// Kudos to a stare-select explanation done for the Google Cardboard by this StackOverflow user:
// https://stackoverflow.com/questions/34384382/use-gaze-input-duration-to-select-ui-text-in-google-cardboard/40842739#40842739
public class Interactible : MonoBehaviour, IFocusable {

    public Shader gazeGlow;
    public Shader selectionGlow;
    public bool beingLookedAt = false;
    public bool isSelected = false;
    public float stareTriggerDuration = 3f;
    private float stareTimer = 0f;
    private GameObject GameController;
    private Color oldColor;
    private static Shader standard;
    
    

    public void OnFocusEnter()
    {
        if (!isSelected)
        {
            GetComponent<Renderer>().material.shader = gazeGlow;
            beingLookedAt = true;
        }


    }



    public void OnFocusExit()
    {
        if (!isSelected)
        {
            GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
            beingLookedAt = false;

        }
    }

    // Use this for initialization
    void Start () {
        GameController = GameObject.Find("GameController");
        standard = Shader.Find("Transparent/Diffuse");
    }
	
	// Update is called once per frame
	void Update () {
		if (beingLookedAt)
        {
            stareTimer += Time.deltaTime;
            if(stareTimer >= stareTriggerDuration)
            {
                
                SelectionManager.HandleSelection(this.gameObject);
                if (this.isSelected)
                {
                    Renderer renderer = GetComponent<Renderer>();
                    renderer.material.shader = selectionGlow;
                    //oldColor = renderer.material.color;
                    //renderer.material.SetColor("_Color", Color.green);

                }
            }
        }
        if(!isSelected)
        {
            Shader currShade = GetComponent<Renderer>().material.shader;
            if(currShade == selectionGlow)
            {
                print("Node " + name + "has been un-selected");
                GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
            }
        }
	}
}
