using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour {

    private LineRenderer lineRenderer;
    private float counter;
    private float distance;
    private float length; // The length of each draw we do.

    // A Transform object is the complete position of an object.
    public Transform origin;
    public Transform destination;

    // How quickly will this line be drawn?
    public float lineDrawSpeed = 6f;

	// Use this for initialization
	void Start () {
        // Always try to cache the component at start - it's expensive to call GetComponent every Update
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, origin.position); // Remember, origin is a Transform - you need to reference its position attribute to get the Vector3 position.

        // How thicc is our line at the beginning and end. Note that these don't need to be the same.
        lineRenderer.startWidth = .05f;
        lineRenderer.endWidth = .05f;

        // How long do we need to draw the line? This is so we can make an animation of the line "reaching out" from the origin to the destination possible.
        distance = Vector3.Distance(origin.position, destination.position);
	}
	
	// Update is called once per frame
	void Update () {
        //if(counter < distance) // In theory, we don't need this, but it will reduce the number of unneccesary checks after the line has been created.
        {
            // Every frame, increment the counter .1 
            counter += .1f / lineDrawSpeed;

            // The length remains dynamic. Mathf.Lerp is a linear interpolation between two positions.
            length = Mathf.Lerp(0, distance, counter);

            Vector3 pointA = origin.position;
            Vector3 pointB = destination.position;

            // Acquire the unit vector (.Normalize) in the direction of the position, then multiply by how long we want the line and add the start point
            Vector3 pointAlongLine = length * Vector3.Normalize(pointB - pointA) + pointA;
            // The one here represents the end of the lineRenderer, where the other end (the one locked at the origin) is at the first object.
            lineRenderer.SetPosition(1, pointAlongLine);
        }

		
	}
}
