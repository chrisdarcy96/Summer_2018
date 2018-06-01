using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class SpawnBall : MonoBehaviour {

    GestureRecognizer recognizer;
    public GameObject ballPrefab;

	// Initialize when script is loaded
	void Awake () {

        recognizer = new GestureRecognizer();
        recognizer.Tapped += Recognizer_TappedEvent;

        recognizer.StartCapturingGestures();
	}

    // https://docs.unity3d.com/Manual/SpatialMappingCollider.html for TappedEventArgs to Ray
    private void Recognizer_TappedEvent(TappedEventArgs args)
    {
        Ray headRay = new Ray(args.headPose.position, args.headPose.forward);
        CreateBouncingBall(headRay);
        print("Tap");


    }

    // https://docs.unity3d.com/ScriptReference/Object.Instantiate.html for object instantiate
    private void CreateBouncingBall(Ray head)
    {
        Vector3 position = head.direction + head.origin * 5.0f; // 2 meters away
        Instantiate(ballPrefab, position, Quaternion.identity);

       
    }


}
