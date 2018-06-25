using System;
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
public class GraphManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        // Associate our DrawCylinders nodes and root with this script
        GameObject nexus = GameObject.Find("LineNexus");
        DrawCylinders dc = nexus.GetComponent<DrawCylinders>();
        List<GameObject> nodes = dc.nodes;
        GameObject root = dc.root;
        print("GraphManager has assigned all DrawCylinder objects correctly.");

    }


    // Update is called once per frame
    void Update()
    {

    }
}
