using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System;
using HoloToolkit.Unity.SpatialMapping;

public class NodeHandler : MonoBehaviour, IFocusable, IInputClickHandler {

    public GameObject spawn;
    public Shader shadeOnView;

    private SubGraphNode[] subNodes;
    private int framesViewed = 0;
    private bool isViewing = false;
    

    // Gaze spawns 3 small spheres at right angles to the focusedObject
    public void OnFocusEnter()
    {
        // change shader
        GetComponent<Renderer>().material.shader = shadeOnView;
        isViewing = !isViewing;
        GraphNodeManager.ToggleSubNodes(this.gameObject);
    }

    private void viewHandler(GameObject focusedObject)
    {
        Transform parent = focusedObject.transform;
        foreach(SubGraphNode sgn in subNodes)
        {
            sgn.hide(false);
        }
        

    }

    public void OnFocusExit()
    {
        isViewing = !isViewing; // toggle isViewing

        GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");

        // if no longer viewing
        if (isViewing == false)
        {

            GraphNodeManager.ToggleSubNodes(this.gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        

        // kill the TapToPlace component
        this.gameObject.GetComponent<TapToPlace>().enabled = false;
        
	}
	
	// Update is called once per frame
	void Update () {
        if (isViewing)
        {
            framesViewed += 1;
        }
		
	}

    public void OnInputClicked(InputClickedEventData eventData)
    {
        // toggle isViewing so to toggle onViewExit behavior
        isViewing = !isViewing;
        this.GetComponent<TapToPlace>().enabled = !this.GetComponent<TapToPlace>().enabled;

        print("Click!!!");
    }
}
