using UnityEngine;
using System.Collections;

/// <summary>
/// Script that makes spheres repel from a central sphere. Based off of Coulomb's Law - with simplifications.
/// Script based off work at https://github.com/kursion/learning-unity/tree/master/unity-repulsion-spheres
/// </summary>

public class SphereRepulsion : MonoBehaviour
{

    // Public variable
    public GameObject SphereCentral; // The attached central sphere
    public float maxRange = 10; // The MAX distance we want any host to move
    [Tooltip("Make sure to only edit this on the prefab - otherwise you may experience unpredictable node behavior"),Range(-10, 10)]
    public float nexusCharge = 2.0F;
    [Tooltip("It is advised that you only edit this on the prefab to avoid unpredictable node behavior"), Range(-10, 10)]
    public float hostCharge = 1.0F;
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

        // If we have attained the distance we desire, cut the force.
        this.currentDistance = Vector3.Distance(this.transform.position, this.SphereCentral.transform.position);
        
        if (this.currentDistance <= )


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