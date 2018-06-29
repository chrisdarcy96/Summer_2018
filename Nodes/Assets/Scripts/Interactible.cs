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
    public Material selectionMaterial;
    public bool beingLookedAt = false;
    public bool isSelected = false;
    public float stareTriggerDuration = 3f;
    private float stareTimer = 0f;
    private GameObject GameController;
    private Color oldColor;
    private Shader standard;
    private Material originMat;
    private Renderer originRender;

    
    

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
        Renderer originRender = GetComponent<Renderer>();
        originMat = originRender.material;
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
                    // Highlight the selected object
                    Renderer renderer = GetComponent<Renderer>();
                    renderer.material = selectionMaterial;
                    renderer.material.shader = selectionGlow;

                    // Clear out the timer and beingLookedAt variables
                    stareTimer = 0f;
                    beingLookedAt = false;
                }
            }
        }
        //if (!isSelected && originRender.material == selectionMaterial)
        //{

        //    print("Node " + name + " has been un-selected");
        //    originRender.material.shader = Shader.Find("Transparent/Diffuse");
        //    originRender.material = originMat;

        //}
    }
}
