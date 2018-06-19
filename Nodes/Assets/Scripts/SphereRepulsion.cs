using UnityEngine;
using System.Collections;

/// <summary>
/// Script that makes spheres repel from a central sphere.
/// Script based off awesome work at https://github.com/kursion/learning-unity/tree/master/unity-repulsion-spheres
/// </summary>

public class SphereRepulsion : MonoBehaviour
{

    // Public variable
    public GameObject SphereCentral; // The attached central sphere
    public float sphereCentralDistance = 1;
    public float sphereSpeed = 0.7F;
    public float fixedY = 1;
    public float currentDistance; // PUBLIC TO DEBUG

    // Private variables
    private bool isTargetReached = true;
    private bool isInRepulsionZone = false; // True when we are in the repulsion zone
    private Vector3 targetedPosition; // Target position from the repulsion effect

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (this.SphereCentral == null) { return; }

        this.currentDistance = Vector3.Distance(this.transform.position, this.SphereCentral.transform.position);


        if (this.currentDistance != this.sphereCentralDistance - 0.01F)
        {
            this.isTargetReached = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // TODO: CHECK TAG FOR CENTRAL SPHERE
        Debug.LogWarning("Trigger Entered between " + this.name+ "and " + other.name);
        if (!IsNexus(other, SphereCentral.name)) { return; }

        
        this.SphereCentral = other.gameObject; // Linking the central sphere gameObject
        this.isInRepulsionZone = true;
        this.isTargetReached = false;

        // Compute new position
        Vector3 position = this.transform.position;
        Vector3 positionSphereCentral = this.SphereCentral.transform.position;
        Vector3 direction = positionSphereCentral - position;
        Vector3 normalized = Vector3.Normalize(direction) * -1; // -1 because we want to repulse to the opposite side

        print(sphereCentralDistance);
        this.targetedPosition = normalized * sphereCentralDistance;
        this.targetedPosition += normalized * 0.01F; // Adding 0.01F because we want the sphere to go out of the zone


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
        this.isInRepulsionZone = false;
    }
}