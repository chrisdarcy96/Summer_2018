using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

//TODO: Potentially may want to rename this.
 
// Kudos to a stare-select explanation done for the Google Cardboard by this StackOverflow user:
// https://stackoverflow.com/questions/34384382/use-gaze-input-duration-to-select-ui-text-in-google-cardboard/40842739#40842739
public class Interactible : MonoBehaviour, IFocusable, IInputClickHandler {

    public Shader gazeGlow;
    public Shader selectionGlow;
    public Material selectionMaterial;
    public bool beingLookedAt = false;
    public bool isSelected = false;
    public float stareTriggerDuration = 3f;
    private float stareTimer = 0f;
    private GameObject GraphManager;

    public GraphController graphController;
    private GraphNodeType gntME;

    private Color oldColor;
    private Shader standard;
    private Material originMat;
    private Renderer originRender;
    private string originTag;

    public void OnFocusEnter()
    {
        beingLookedAt = true;
        if (!isSelected)
        {
            GetComponent<Renderer>().material.shader = gazeGlow;
            
        }


    }



    public void OnFocusExit()
    {
        beingLookedAt = false;
        if (!isSelected)
        {
            GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
            

        }
    }

    // Use this for initialization
    void Start () {
        GraphManager = GameObject.Find("GraphManager");
        graphController = GraphManager.GetComponent<GraphController>();
        standard = Shader.Find("Transparent/Diffuse");
        originRender = GetComponent<Renderer>();
        originMat = originRender.material;
        originTag = gameObject.tag;
        //print("Orginal tag for " + name + " is " + originTag);

        // store the GraphNodeType for this object
        foreach(GraphNodeType gnt in graphController.Nodes)
        {
            if (GameObject.ReferenceEquals(this.gameObject, gnt.getObject()))
            {
                gntME = gnt;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (beingLookedAt)
        {
            stareTimer += Time.deltaTime;
            if(stareTimer >= stareTriggerDuration)
            {
                ToggleSelection();
            }
        }
        else
        {
            stareTimer = 0.0f;
        }


    }

    private void SelectSelf()
    {
        isSelected = true;
        gameObject.tag = "selected";
        // Set the selection material defined in the SelectionManager
        originRender.material = selectionMaterial;
        originRender.material.shader = selectionGlow;

        graphController.ToggleSubNodes(gameObject);
        
    }

    public void UnSelectSelf()
    {
        //print("UnSelectSelf() called");
        isSelected = false;
        originRender.material = originMat;
        originRender.material.shader = standard;
        gameObject.tag = originTag;
        // Toggle the viewing of the subnodes by reaching out to the graphManager
        gntME.ToggleActiveSubs();
    }
    public void ToggleSelection()
    {
        // restart timer, we are counting again...
        stareTimer = 0f;
        if (isSelected)
        {
            UnSelectSelf();
        }
        else if (!isSelected)
        {
            SelectSelf();
        }
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        ToggleSelection();
    }
}
