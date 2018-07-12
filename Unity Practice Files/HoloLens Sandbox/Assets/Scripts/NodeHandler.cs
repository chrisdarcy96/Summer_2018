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
    private ArrayList spawnsThisView;
    private int framesViewed = 0;
    private bool isViewing = false;
    private TextMesh textGUI;

    // Gaze spawns 3 small spheres at right angles to the focusedObject
    public void OnFocusEnter()
    {
        // change shader
        GetComponent<Renderer>().material.shader = shadeOnView;
        isViewing = !isViewing;
        textGUI.text = "Viewing: " + this.gameObject.tag + "_" + this.gameObject.GetInstanceID();
        viewHandler(this.gameObject);
    }

    private void viewHandler(GameObject focusedObject)
    {
        Transform parent = focusedObject.transform;
       
        // create 3 miniNodes of Information
        GameObject newMiniNode = Instantiate(spawn); 
        spawnsThisView.Add(newMiniNode);
        // right top
        newMiniNode.transform.position = new Vector3(parent.position.x + 4 * newMiniNode.GetComponent<Renderer>().bounds.size.x, parent.position.y 
                                                    + 4 * newMiniNode.GetComponent<Renderer>().bounds.size.y, parent.position.z);

        newMiniNode = Instantiate(spawn); // create new spawn
        spawnsThisView.Add(newMiniNode);
        // right middle
        newMiniNode.transform.position = new Vector3(parent.position.x + 4 * newMiniNode.GetComponent<Renderer>().bounds.size.x, 
                                                    parent.position.y, parent.position.z);

        newMiniNode = Instantiate(spawn); // create new spawn
        spawnsThisView.Add(newMiniNode);
        //right bottom
        newMiniNode.transform.position = new Vector3(parent.position.x + 4 * newMiniNode.GetComponent<Renderer>().bounds.size.x, 
                                                     parent.position.y - 4 * newMiniNode.GetComponent<Renderer>().bounds.size.y, parent.position.z);
    
    }

    public void OnFocusExit()
    {
        isViewing = !isViewing; // toggle isViewing

        GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");

        // if no longer viewing
        if(isViewing == false)
        {
            textGUI.text = "";
            framesViewed = 0;
            // clear array list of spawned objects
            for (int i = 0; i < spawnsThisView.Count; i++)
            {
                GameObject toDestroy = (GameObject)spawnsThisView[i];
                Destroy(toDestroy);
            }
            spawnsThisView.Clear();
        }
        
    }

    // Use this for initialization
    void Start () {
        // Find Object named "Hinter" and get its text mesh
        try
        {
            textGUI = GameObject.Find("Hinter").GetComponent<TextMesh>();
        }
        catch
        {
            throw new Exception("GameObject Hinter does not exist in this scene.");
        }

        // so we know its working
        textGUI.text = "Working...";

        spawnsThisView = new ArrayList();

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
