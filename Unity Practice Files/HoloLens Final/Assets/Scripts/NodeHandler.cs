using UnityEngine;
using HoloToolkit.Unity.InputModule;
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
        // don't do anything if they are already viewing...
        if (!isViewing)
        {
            isViewing = !isViewing;
            GetComponent<Renderer>().material.shader = shadeOnView;
            GraphNodeManager.ToggleSubNodes(this.gameObject);
        }
        else
        {
            isViewing = !isViewing; // set to false to keep active on exit :)
        }
    }


    public void OnFocusExit()
    {
        // if the object was not selected
        if (isViewing)
        {
            GraphNodeManager.ToggleSubNodes(this.gameObject);
            GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
            isViewing = false;  //no longer viewing, 
        }
        // if selected
        else if (!isViewing)
        {
            isViewing = true;   // set back to true for OnFocusEnter again
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
        //if(shadeOnView.ToString() == GetComponent<Renderer>().material.shader.ToString())
        //{
        //    Debug.Log("Success");
        //}

	}

    public void OnInputClicked(InputClickedEventData eventData)
    {
        // toggle isViewing so to toggle onViewExit behavior
        isViewing = !isViewing;

    }
}
