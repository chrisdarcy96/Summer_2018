using System;
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
            connectors[i].GetComponent<Renderer>().material = lineMaterial;

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

        }

    }
	
	// Update is called once per frame
	void Update () {

        UpdatePositions(nodes, connectors);
		
	}

    private void UpdatePositions(GameObject[] nodes, GameObject[] connectors)
    {

        // Update our start vector, in case the root has moved.
        Vector3 updateStart = root.transform.position;
        for (int j = 0; j < nodes.Length; j++)
        {
            Vector3 updateEnd = nodes[j].transform.position;
            Vector3 updateMidpoint = (updateEnd - updateStart) * 0.5f + updateStart;
            distance = Vector3.Distance(updateStart, updateEnd);

            // Put it at the midpoint.
            connectors[j].transform.position = updateMidpoint;

            // TODO: See if we really need to re-do all these calcs each update or if we can store some positional data.

            Vector3 scale = new Vector3(radius, distance / 2, radius);
            connectors[j].transform.localScale = scale;

            // Orient the cylinder.
            connectors[j].transform.rotation = Quaternion.FromToRotation(Vector3.up, updateEnd - updateStart);
        }
        // TODO: Check and destroy any extra cylinders
    }
}

