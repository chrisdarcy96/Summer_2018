using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCursor : MonoBehaviour {

    private MeshRenderer ms;
	// Use this for initialization
	void Start () {
        // get mesh renderer on the same game object as this script
        ms = this.gameObject.GetComponentInChildren<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        // raycast into world based on user's head
        // position and direction
        var headPos = Camera.main.transform.position;
        var gazeDir = Camera.main.transform.forward;

        RaycastHit hitInfo;

        if(Physics.Raycast(headPos, gazeDir, out hitInfo))
        {
            // if raycast hit a hologram...
            ms.enabled = true; // display cursor mesh
            this.transform.position = hitInfo.point; // move cursor to raycast hit location
            this.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal); // rotate cursor normal (hugging) surface of hologram
        }
        else
        {
            ms.enabled = false; // if raycast did not hit, hide cursor mesh
        }
	}
}
