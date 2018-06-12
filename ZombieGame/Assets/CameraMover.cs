using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour {

    public float cameraSensitivity = 0.9f;
    Camera cam;

	// Use this for initialization
	void Start () {
        cam = this.GetComponent<Camera>();
        cam.transform.rotation = new Quaternion(0, 0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        // Camera follows mouse pointer
        Vector3 smoothCam = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
        smoothCam.x -= cameraSensitivity;
        smoothCam.y -= cameraSensitivity;

        smoothCam.x *= cameraSensitivity;
        smoothCam.y *= cameraSensitivity;

        smoothCam.x += cameraSensitivity;
        smoothCam.y += cameraSensitivity;


        transform.LookAt(smoothCam, Vector3.up);


        restrictLooping();
	}

    void restrictLooping()
    {
        
    }
}
