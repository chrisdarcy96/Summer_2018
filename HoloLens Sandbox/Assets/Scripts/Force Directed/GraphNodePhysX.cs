using UnityEngine;


public class GraphNodePhysX : GraphNode {

    private Rigidbody thisRigidbody;
    private float sphRadius;
    private float sphRadiusSqr;

    protected override void DoGravity()
    {
        // Apply global gravity pulling node towards center of universe
        Transform manage = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 dirToCenter = manage.position - this.transform.position;
        Vector3 impulse = dirToCenter.normalized * thisRigidbody.mass * manager.Gravity_PhysX;
        thisRigidbody.AddForce(impulse);
    }

    protected override void DoRepulse()
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
                Vector3 newForce = direction.normalized * manager.Repulse_Force_Strength * impulseExpoFalloffByDist;
                if(float.IsNaN(newForce.x) || float.IsNaN(newForce.y) || float.IsNaN(newForce.z))
                {
                    newForce = Vector3.zero;
                }
                // apply normalized distance
                hitRb.AddForce(newForce);
                
            }
        }
    }

    protected override void DoFreeze()
    {   // toggle movement
        thisRigidbody.isKinematic = !thisRigidbody.isKinematic;
    }

    protected override void Start()
    {
        base.Start(); // still call override start
        thisRigidbody = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        // updating variable here, as opposed to doing it in Start(), otherwise we won't see runtime updates of forceSphereRadius
        sphRadius = manager.Force_Sphere_Radius;
        sphRadiusSqr = sphRadius * sphRadius;

        // prevent node from straying too far from home
        Vector3 home = GameObject.FindGameObjectWithTag("Player").transform.position;
        if( this.transform.position.x > home.x + .5 || this.transform.position.x < home.x - .5)
        {
            thisRigidbody.constraints = RigidbodyConstraints.FreezePositionX;
        }
        if(this.transform.position.y > home.y + .5 || this.transform.position.y < home.y - .1)
        {
            thisRigidbody.constraints = RigidbodyConstraints.FreezePositionY;
        }
        if(this.transform.position.z > home.z + 2 || this.transform.position.z < home.z)
        {
            thisRigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
        }
    }
}