using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCylinders : MonoBehaviour {
    public Material lineMaterial;
    public float radius = 0.05f;
    public GameObject root;
    public GameObject[] nodes;
    private GameObject[] connectors;
    private Transform target;
    private float distance;
    private int i = 0;

	// Use this for initialization
	void Start () {
        connectors = new GameObject[nodes.Length];

        // Line up the nexus and the root
        this.gameObject.transform.position = root.transform.position;
        print("Nexus gameObject moved to " + this.gameObject.transform.position);

        // Save the root position for creating nodes
        Vector3 start = root.transform.position;
        print("Start vector at " + start);

        // Create a cylinder for each connection
        for (i=0; i < nodes.Length; i++)
        {
            // Calculate the half-distance for each pair of points and put a cylinder at each place
            
            Vector3 end = nodes[i].transform.position;
            print("End vector at " + end);
            Vector3 midpoint = (end - start) * 0.5f + start;
            print("Midpoint between root and node " + i + " is " + midpoint);
            distance = Vector3.Distance(start, end);
            print("Distance between start and end: " + distance);

            //Create a cylinder
            connectors[i] = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            // Name and assign the cylinder
            connectors[i].name = "Connector" + i;
            connectors[i].transform.parent = this.gameObject.transform;

            // Put it at the midpoint.
            connectors[i].transform.position = midpoint;
            print("Connector position set to " + midpoint);

            Vector3 scale = new Vector3(radius, distance/2, radius);
            print("Scale Vector resolves to " + scale);
            connectors[i].transform.localScale = scale;

            // Orient the cylinder.
            connectors[i].transform.rotation = Quaternion.FromToRotation(Vector3.up, end - start);

            

            // GameObject-Cylinder implementation
            //print("Cylinder creation loop: " + i);
            //this.connectors[i] = new GameObject(); // This is the parent object
            //this.connectors[i].name = "Connector" + i;
            //print("Created: " + this.connectors[i].name);
            //this.connectors[i].transform.position = root.transform.position;
            //this.connectors[i].transform.parent = this.gameObject.transform;
            //// Currently, this cylinder will rotate about its center - which we want to turn into the cylinder rotating around the center of its base.
            //// That's why it's a GameObject going into the array and not the primitive itself.
            //GameObject actualCylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            //actualCylinder.transform.parent = this.connectors[i].transform;

            //// Offset so we can rotate the cylinders from the bottom in the future.
            //// TODO: Ensure this is relative to the main position
            //actualCylinder.transform.localPosition = new Vector3(root.transform.position.x, 1f, root.transform.position.z);  // Make sure the cube is initially locked to the root!
            //// Scale the radius
            //actualCylinder.transform.localScale = new Vector3(radius, 1f, radius);
            ////set the material of the cylinder
            //actualCylinder.GetComponent<Renderer>().material = lineMaterial;
        }

    }
	
	// Update is called once per frame
	void Update () {

        // Iterate through every point and have each cylinder object look at and transform to that point.
        //for(i=0;i<nodes.Length;i++)
        //{
        //    target = nodes[i].transform;
        //    // Orient for the connection
        //    Transform currentConnector = this.connectors[i].transform;
        //    currentConnector.LookAt(target, Vector3.up);
        //    distance = Vector3.Distance(root.transform.position, target.position);

        //    // Make the connection
        //    currentConnector.localScale = new Vector3(currentConnector.localScale.x, distance, currentConnector.localScale.y);

        //}
		
	}
}
