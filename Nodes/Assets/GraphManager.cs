using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * GraphManager Class
 * This class manages the movement of the children nodes of the DrawCylinder Class as well as their default behavior.
 */
/// <summary>
/// This class manages the movement of the children nodes of the DrawCylinder Class as well as their default behavior.
/// </summary>
/// <author>
/// James Lynch
/// </author>
public class GraphManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // Associate our DrawCylinders nodes with this script
        GameObject nexus = GameObject.Find("Nexus");
        List<GameObject> nodes = nexus.GetComponent<DrawCylinders>().nodes;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
