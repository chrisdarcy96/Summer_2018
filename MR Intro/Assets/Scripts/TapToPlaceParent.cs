using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToPlaceParent : MonoBehaviour {

    public bool placing = false;
	// Use this for initialization
	void OnSelect () {
        placing = !placing; // toggle placing mode

        if (placing)
        {
            SpatialMapping.Instance.DrawVisualMeshes = true;
        }
        else
        {
            SpatialMapping.Instance.DrawVisualMeshes = false;
        }

	}
	
	// Update is called once per frame
	void Update () {

        // create raycast that only hits spatial mapping mesh
        Vector3 headPos = Camera.main.transform.position;
        Vector3 gazeDir = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if(Physics.Raycast(headPos, gazeDir, out hitInfo, 30.0f, SpatialMapping.PhysicsRaycastMask))
        {
            // move object parent to raycast hit on spatial mapping mesh
            this.transform.parent.position = hitInfo.point;

            // and rotate to face user
            Quaternion toQuat = Camera.main.transform.localRotation;
            toQuat.x = 0;
            toQuat.y = 0;
            this.transform.parent.rotation = toQuat;
        }

	}
}
