using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceDirectionManager : MonoBehaviour {

    [SerializeField] private bool repulseActive = true;
    [SerializeField] private bool gravityActive = false;
    [SerializeField] private float gravity_PhysX = 15f;
    [SerializeField] private float repulse_Force_Strength = .05f;
    [SerializeField] private float force_Sphere_Radius = 2.5f;

    public bool RepulseActive
    {
        get
        {
            return repulseActive;
        }
        set
        {
            repulseActive = value;
        }
    }

    public bool GravityActive
    {
        get
        {
            return gravityActive;
        }
        set
        {
            gravityActive = value;
        }
    }
    
    public float Gravity_PhysX
    {
        get
        {
            return gravity_PhysX;
        }
    }

    public float Repulse_Force_Strength
    {
        get
        {
            return repulse_Force_Strength;
        }
    }

    public float Force_Sphere_Radius
    {
        get
        {
            return force_Sphere_Radius;
        }
    }

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
