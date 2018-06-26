using System;
using UnityEngine;

public class GraphNodeType : MonoBehaviour {

    
    private GameObject thisObject;
    private DateTime nodeTime;
    private bool IsActive = false;
    private Vector3 position;
    private string hostIP;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
       
	}

    // ************ Accessors *****************
    public GameObject getObject()
    {
        return thisObject;
    }

    public bool getActive()
    {
        return IsActive;
    }

    public DateTime getTime()
    {
        return nodeTime;
    }

    public Vector3 getPosition()
    {
        return position;
    }

    public string getHost()
    {
        return hostIP;
    }

    // ************ Mutators *****************
    public void setTime(DateTime newTime)
    {
        nodeTime = newTime;
    }

    public void setActive(bool newActive)
    {
        if (newActive != IsActive)
        {
            // change mesh if there is a change
            thisObject.GetComponent<Renderer>().enabled = newActive;
        }
        IsActive = newActive;
    }

    public void setObject(GameObject newObject)
    {
        thisObject = newObject;
    }

    public void setPosition(Vector3 newPos)
    {
        position = newPos;  // set position
        // now move object to that position
        thisObject.transform.position = position;
    }

    public void setHost(string newHost)
    {
        hostIP = newHost;
    }

}
