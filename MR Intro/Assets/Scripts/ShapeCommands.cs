using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeCommands : MonoBehaviour {
    private bool isFalling = false;
	// called when selected from GazeGestureManager.cs
    void OnSelect()
    {
        // if it doesn't already have RigidBody component...
        if (!this.GetComponent<Rigidbody>())
        {
            Rigidbody rg = this.gameObject.AddComponent<Rigidbody>();
            rg.collisionDetectionMode = CollisionDetectionMode.Continuous;
            isFalling = true;
        }
    }

    void OffSelect()
    {
        // reverse gravity, should already have RigidBody Component
        Rigidbody rg = this.gameObject.GetComponent<Rigidbody>();
        rg.AddForce(Physics.gravity * -2);
        isFalling = false;
    }
    
    void Update()
    {
        if (this.transform.position.y >= 0.05 && !isFalling)
        {
            this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); // kill all movement
            // kill gravity
            Destroy(this.GetComponent<Rigidbody>()); // remove the rigidbody
        }
    }
}
