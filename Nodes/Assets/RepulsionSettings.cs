using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script allows multiple hosts to be nested under a single parent GameObject that standardizes charge relationships between all the nodes.
/// </summary>



public class RepulsionSettings : MonoBehaviour {
    [Tooltip("Set the repulsive (close) charge power of the central object"), Range(-10, 10)]
    public float nexusCharge = 2.0F;
    [Tooltip("Set the attractive (far) charge power of the central object"), Range(-10, 0)]
    public float gravity = -1.0F;
    [Tooltip("Set the charge power of a host object"), Range(-10, 10)]
    public float hostCharge = 1.0F;
    [Tooltip("Use to magnify or decrease the power of all forces")]
    public float scale = 1.0f;
    [Tooltip("This defines the max distance between a host and the central object before SphereRepulsion.cs stops the hosta")]
    public float maxRange = 2.5f;
    public GameObject sphereCentral; // The attached central sphere
    [Tooltip("The collider for the gameobject here will act as an attractive zone - should be coincident with the central sphere.")]
    public GameObject boundary; 

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
