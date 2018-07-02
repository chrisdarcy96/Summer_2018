using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour {
    private List<GameObject> nodes = new List<GameObject>();
    public static GameObject currentlySelected = null;
    public static Material selectionMaterial;

    // Use this for initialization
    void Start () {
        // Link with the GraphController's Node list
        nodes = GetComponent<GraphController>().nodes;
	}
	
	// Update is called once per frame
	void Update () {

        // Main purpose of this update loop is to find a new node with isSelected and manage the handoff.
        
	}

    public static void HandleSelection(GameObject newSelection)
    {
        if (newSelection == currentlySelected)
        {
            print("SelectionManager Ignoring duplicate selection.");
            return;
        }
        // At this point we will go ahead ("approve" the selection)
        Interactible nsinteractible = newSelection.GetComponent<Interactible>();

        // Un-select if something else is currently selected.
        if (currentlySelected != null)
        {
            print("SelectionManager un-selecting " + currentlySelected.name);
            Interactible csinteractible = currentlySelected.GetComponent<Interactible>();
            csinteractible.isSelected = false;
            // The Interactible script of the formerly selected node will re-assign its own materials and tags
   
        }
        currentlySelected = newSelection;
        nsinteractible.isSelected = true;
        nsinteractible.tag = "selection";
    }
}
