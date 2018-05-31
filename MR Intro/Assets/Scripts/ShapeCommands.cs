using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeCommands : MonoBehaviour {

    Vector3 origPos;

    void Start()
    {
        // save starting position
        origPos = this.transform.localPosition;
    }
    
	// called when selected from GazeGestureManager.cs
    void OnSelect()
    {
        // if it doesn't already have RigidBody component...
        if (!this.GetComponent<Rigidbody>())
        {
            Rigidbody rg = this.gameObject.AddComponent<Rigidbody>();
            rg.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }

    void OnReset()
    {   
        // disable physics
        Rigidbody rigBody = this.GetComponent<Rigidbody>();
        if(rigBody != null)
        {
            rigBody.isKinematic = true;
            Destroy(rigBody);
        }
        this.transform.localPosition = origPos;
    }

    void OnDrop()
    {
        OnSelect();
    }

}
