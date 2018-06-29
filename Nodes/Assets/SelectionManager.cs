using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour {
    private List<GameObject> nodes = new List<GameObject>();
    public static GameObject currentlySelected = null;
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
        if(newSelection == currentlySelected)
        {
            return;
        }
        if(currentlySelected != null)
        {
            currentlySelected.GetComponent<Interactible>().isSelected = false;
        }
        currentlySelected = newSelection;
        newSelection.GetComponent<Interactible>().isSelected = true;
    }
}
