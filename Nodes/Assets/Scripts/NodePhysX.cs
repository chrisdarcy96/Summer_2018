﻿using UnityEngine;
using System.Collections;

public class NodePhysX : Node {

    private Rigidbody thisRigidbody;

    private float sphRadius;
    private float sphRadiusSqr;
    public  bool delete = false;
    public bool hide = false;

    // Public debug settings
    [Tooltip("This offsets the center of gravity for the graph in the z direction. Default is 3 (roughly standard for HoloLens)")]
    //public float zOffset = 3;
    public GameObject root;

    protected override void doGravity()
    {
        // Apply global gravity pulling node towards center of universe
        //Vector3 dirToCenter = - this.transform.position + new Vector3(0,0,zOffset); // For the HoloLens, the "center" is (0,0,2)
        Vector3 dirToCenter = -this.transform.position + root.transform.position; // For the HoloLens, the "center" is (0,0,2)

        Vector3 impulse = dirToCenter.normalized * thisRigidbody.mass * graphControl.GlobalGravityPhysX;
        thisRigidbody.AddForce(impulse);
    }

    protected override void doRepulse()
    {
        // test which node in within forceSphere.
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, sphRadius);

        // only apply force to nodes within forceSphere, with Falloff towards the boundary of the Sphere and no force if outside Sphere.
        foreach (Collider hitCollider in hitColliders)
        {
            Rigidbody hitRb = hitCollider.attachedRigidbody;

            if (hitRb != null && hitRb != thisRigidbody)
            {
                Vector3 direction = hitCollider.transform.position - this.transform.position;
                float distSqr = direction.sqrMagnitude;

                // Normalize the distance from forceSphere Center to node into 0..1
                float impulseExpoFalloffByDist = Mathf.Clamp( 1 - (distSqr / sphRadiusSqr), 0, 1);

                // apply normalized distance
                hitRb.AddForce(direction.normalized * graphControl.RepulseForceStrength * impulseExpoFalloffByDist);
            }
        }
    }

    protected override void Start()
    {
        base.Start();
        thisRigidbody = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        // updating variable here, as opposed to doing it in Start(), otherwise we won't see runtime updates of forceSphereRadius
        sphRadius = graphControl.NodePhysXForceSphereRadius;
        sphRadiusSqr = sphRadius * sphRadius;
    }
}