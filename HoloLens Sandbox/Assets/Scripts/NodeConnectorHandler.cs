using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Code Adapted from C19 James Lynch
 * June 2018
 */

public class NodeConnectorHandler : MonoBehaviour
{
    public Material lineMaterial;
    public float radius = 0.05f;
    public GameObject root;
    public GameObject[] nodes;

    private List<GameObject> connectors = new List<GameObject>();
    private float distance;



    // Use this for initialization
    void Start()
    {
        nodes = TextToView.Nodes;

        // Line up the nexus and the root
        this.gameObject.transform.position = root.transform.position;
        // print("Nexus gameObject moved to " + this.gameObject.transform.position);

        // Save the root position for creating nodes
        Vector3 start = root.transform.position;
        // print("Start vector at " + start);

        // Create a cylinder for each connection
        for(int i=0; i<nodes.Length; i++)
        {
            // calculate midpoints
            Vector3 end = nodes[i].transform.position;
            //print("End vector at " + end);
            Vector3 midpoint = (end - start) * 0.5f + start;
            //print("Midpoint between root and node " + i + " is " + midpoint);
            distance = Vector3.Distance(start, end);
            //print("Distance between start and end: " + distance);

            //Create a cylinder
            connectors.Add(GameObject.CreatePrimitive(PrimitiveType.Cylinder));

            // Name and assign the cylinder
            connectors[connectors.Count - 1].name = "Connector" + (connectors.Count - 1);
            connectors[connectors.Count - 1].transform.parent = this.gameObject.transform;
            connectors[connectors.Count - 1].gameObject.GetComponent<Renderer>().material = lineMaterial;

            // Put it at the midpoint.
            connectors[connectors.Count - 1].transform.position = midpoint;
            //print("Connector position set to " + midpoint);

            Vector3 scale = new Vector3(radius, distance / 2, radius);
            connectors[connectors.Count - 1].transform.localScale = scale;

            // Orient the cylinder.
            connectors[connectors.Count - 1].transform.rotation = Quaternion.FromToRotation(Vector3.up, end - start);

        }

    }

    // Update is called once per frame
    void Update()
    {
        // Check whether or not we need to add or remove cylinders
        UpdateCylinders(nodes, connectors);
        UpdatePositions(nodes, connectors);
    }

    private void UpdateCylinders(GameObject[] nodes, List<GameObject> connectors)
    {

        //// Wipe any nulls from the nodes list
        if (nodes.Contains(null))
        {
        //    print("Trying to clean up nodes list...");
            //nodes.RemoveAll(item => item == null); // new lambda syntax. Neat!
        }

        if (connectors.Count < nodes.Length)
        {
            // FIXME: Watch out for a potential garbage collector issue detailed here: https://answers.unity.com/questions/1315488/destroy-a-gameobject-referenced-in-list-and-remove.html
            // If we have too few Cylinders, then we need to add more
            while (connectors.Count < nodes.Length)
            {
                // print("Adding connector...");

                // Note that we are just adding cylinders to the array - they'll remain at the origin until the UpdatePositions call in the same Update
                connectors.Add(GameObject.CreatePrimitive(PrimitiveType.Cylinder));

                // Name and assign the cylinder
                connectors[connectors.Count - 1].name = "Connector" + (connectors.Count - 1);
                connectors[connectors.Count - 1].transform.parent = this.gameObject.transform;
            }
        }
        else if (connectors.Count > nodes.Length)
        {
            // If we have too many cylinders, then we need to remove some
            while (connectors.Count > nodes.Length)
            {
                // print("Removing connector...");
                Destroy(connectors[connectors.Count - 1]);
                connectors.RemoveAt(connectors.Count - 1);
            }
        }
    }

    private void UpdatePositions(GameObject[] nodes, List<GameObject> connectors)
    {

        // Update our start vector, in case the root has moved.
        // TODO: is there a potential case where we can still enter this function with mismatching node/connector lists?
        Vector3 updateStart = root.transform.position;

        if (nodes.Contains(null))
        {
            print("There's a null in the nodes list!");
        }

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
