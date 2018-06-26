using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GraphNode : GraphNodeType {

    private GameObject thisNode;    // GameObject
    private Time nodeTime;          // Splunk time of connection
    private bool isActive = false;  // default to false

    //protected override void Hide()
    //{
    //    thisNode.GetComponent<MeshRenderer>().enabled = false;
    //}

    //protected override void Show()
    //{
    //    thisNode.GetComponent<MeshRenderer>().enabled = true;
    //}


	// Use this for initialization
	void Start () {
        //base.Start();   // call base start still
        thisNode = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
