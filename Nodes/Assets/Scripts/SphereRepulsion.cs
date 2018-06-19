using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Script that makes spheres repel from a central sphere. Based off of Coulomb's Law - with simplifications.
/// Script based off work at https://github.com/kursion/learning-unity/tree/master/unity-repulsion-spheres
/// </summary>

public class SphereRepulsion : MonoBehaviour
{
    private const double Y = 2.0;

    // Public variable
    public GameObject SphereCentral; // The attached central sphere
    public float maxRange = 10; // The MAX distance we want any host to move
    [Tooltip("Make sure to only edit this on the prefab - otherwise you may experience unpredictable node behavior"),Range(-10, 10)]
    public float nexusCharge = 2.0F;
    [Tooltip("It is advised that you only edit this on the prefab to avoid unpredictable node behavior"), Range(-10, 10)]
    public float hostCharge = 1.0F;
    public float fixedY = 1;
    public float currentDistance; // PUBLIC FOR DEBUGGING


    // Private variables
    private bool isTargetReached = true;
    private bool influenced = false; // True when we are in the repulsion zone
    private List<GameObject> influencers = new List<GameObject>();
    private double k = 8.99e9;


    // Use this for initialization
    void Start()
    {
        // Adjust the size of the collider based on the given charge value
    }

    // Update is called once per frame
    void Update()
    {
        if (this.SphereCentral == null) { return; }

        // If we have attained the distance we desire, cut the force.
        this.currentDistance = Vector3.Distance(this.transform.position, this.SphereCentral.transform.position);
        if(this.currentDistance >= maxRange)
        {
            forceHalt(this);
        }
        //Otherwise, we want to calculate the estimated force based off of all units within our collider.
        calcForce(influencers);


    }

    private void calcForce(List<GameObject> influencers)
    {

        float otherCharge = 1;
        Vector3 netForce = new Vector3(); 

        // Create a net charge force in 3d based off of the influence of multiple other gameObjects
        foreach(GameObject influencer in influencers)
        {
            // find a vector force inversely proportional to distance
            Vector3 compForce = new Vector3();
            double distance = Vector3.Distance(this.transform.position, influencer.transform.position);
            compForce = (this.transform.position - influencer.transform.position).normalized; // start with a unit vector pointing from one to another.
            
            // Use the nexusCharge if the other object is the nexus; otherwise we have the charge
            if(influencer.name == "LineNexus")
            {
                otherCharge = nexusCharge;
            }
            else
            {
                otherCharge = this.hostCharge;
            }

            double scalarForce = (this.hostCharge * otherCharge * k) / Math.Pow(distance, 2);
            // TODO: Figure out a nicer way to do this than grind a double down to a float
            compForce *= (float)scalarForce;

            print("Force magnitude is " + scalarForce);
            netForce += (compForce);


            

        }
    }

    private void forceHalt(SphereRepulsion sphereRepulsion)
    {
        throw new NotImplementedException();
    }

    void OnTriggerEnter(Collider other)
    {
        // TODO: CHECK TAG FOR CENTRAL SPHERE
        Debug.LogWarning("Trigger Entered between " + this.name+ "and " + other.name);
        if (!IsNexus(other, SphereCentral.name)) { return; }

        
        this.SphereCentral = other.gameObject; // Linking the central sphere gameObject
        this.influenced = true;
        this.isTargetReached = false;

        // Compute new position
        Vector3 position = this.transform.position;
        Vector3 positionSphereCentral = this.SphereCentral.transform.position;
        Vector3 direction = positionSphereCentral - position;
        Vector3 normalized = Vector3.Normalize(direction) * -1; // -1 because we want to repulse to the opposite side

        // Throw the new collider into the list of objects that this cares about
        if (!influencers.Contains(other.gameObject))
        {
            influencers.Add(other.gameObject);
        }

    }

    private static bool IsNexus(Collider other, string name)
    {
        if (other.name == null || name == null)
        {
            return false;
        }
        return other.transform.parent.gameObject.name == name;
    }

    void OnTriggerExit(Collider other)
    {
        // Snap the position to the exact value
        Debug.LogWarning("EXITING TRIGGER ZONE");
        this.influenced = false;

        // Remove the influencer if it's in the influencers list
        if (influencers.Contains(other.gameObject))
        {
            influencers.Add(other.gameObject);
        }

    }
}