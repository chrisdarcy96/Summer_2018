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
    public GameObject sphereCentral; // The attached central sphere
    public GameObject settingsParent; 
    float currentDistance; // PUBLIC FOR DEBUGGING


    // A parent object with RepulsionSettings needs 




    // Private fields
    private bool isTargetReached = true;
    private bool influenced = false; // True when we are in the repulsion zone
    private List<GameObject> influencers = new List<GameObject>();
    private float maxRange, hostCharge, nexusCharge, scale;

    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        // Select our parent settings object (which has RepulsionSettings.cs as a component)
        RepulsionSettings settings = this.GetComponentInParent<RepulsionSettings>();
        // TODO: Adjust the size of the collider based on the given charge value
        rb = GetComponent<Rigidbody>();
        nexusCharge = settings.nexusCharge;
        hostCharge = settings.hostCharge;
        maxRange = settings.maxRange;
        float scale = settings.scale;
}

    // Update is called once per frame
    void Update()


    {
        if (this.sphereCentral == null) { return; }

        // If we have attained the distance we desire, cut the force.
        this.currentDistance = Vector3.Distance(this.transform.position, this.sphereCentral.transform.position);
        print("Current distance is: " + this.currentDistance + " and the max Range is: " + maxRange);
        if (this.currentDistance >= maxRange)
        {
            Debug.LogWarning("Entering halt function for " + this.gameObject.name);
            forceHalt(this.gameObject);
        }
        //Otherwise, we want to calculate the estimated force based off of all units within our collider.
        // ... but only if there's any influencers 

        if (influencers.Count > 0) { calcForce(influencers); }



    }

    private void calcForce(List<GameObject> influencers)
    {

        float otherCharge = 1;
        Vector3 netForce = new Vector3();

        // Create a net charge force in 3d based off of the influence of multiple other gameObjects
        foreach (GameObject influencer in influencers)
        {
            // find a vector force inversely proportional to distance
            Vector3 compForce = new Vector3();
            double distance = Vector3.Distance(this.transform.position, influencer.transform.position);
            compForce = (this.transform.position - influencer.transform.position).normalized; // start with a unit vector pointing from one to another.

            // Use the nexusCharge if the other object is the nexus; otherwise we have the charge
            if (influencer.name == "LineNexus")
            {
                otherCharge = nexusCharge;
            }
            else
            {
                otherCharge = this.hostCharge;
            }

            double scalarForce = (this.hostCharge * otherCharge * scale) / Math.Pow(distance, 2);
            // TODO: Figure out a nicer way to do this than grind a double down to a float
            compForce *= (float)scalarForce;

            print("Force magnitude is " + scalarForce);
            netForce += (compForce);

        }

        print("Force calculated successfully: " + netForce.x + "/" + netForce.y + "/" + netForce.z);
        rb.AddForce(netForce);

    }

    private void forceHalt(GameObject obj)
    {
        ///<summary>
        ///Simply stops a gameobject
        /// </summary>
        /// 
        Debug.LogWarning("Halting " + obj.name);
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    void OnTriggerEnter(Collider other)
    {
        // TODO: CHECK TAG FOR CENTRAL SPHERE
        Debug.LogWarning("Trigger Entered between " + this.name + "and " + other.name);
        //if (!IsNexus(other, SphereCentral.name)) { return; }


        this.sphereCentral = other.gameObject; // Linking the central sphere gameObject
        this.influenced = true;
        this.isTargetReached = false;

        // Compute new position
        Vector3 position = this.transform.position;
        Vector3 positionSphereCentral = this.sphereCentral.transform.position;
        Vector3 direction = positionSphereCentral - position;
        Vector3 normalized = Vector3.Normalize(direction) * -1; // -1 because we want to repulse to the opposite side

        // Throw the new collider into the list of objects that this cares about
        if (!influencers.Contains(other.gameObject))
        {
            influencers.Add(other.gameObject);
            Debug.LogWarning("Influencer added to " + this.name + ":  " + other.name);

        }

    }

    //private static bool IsNexus(Collider other, string name)
    //{
    //    if (other.name == null || name == null)
    //    {
    //        return false;
    //    }
    //    return other.transform.parent.gameObject.name == name;
    //}

    void OnTriggerExit(Collider other)
    {
        // Snap the position to the exact value
        Debug.LogWarning("EXITING TRIGGER ZONE");
        this.influenced = false;

        // Remove the influencer if it's in the influencers list
        if (influencers.Contains(other.gameObject))
        {
            influencers.Add(other.gameObject);
            Debug.LogWarning("Influencer removed from " + this.name + ":  " + other.name);

        }

    }
}