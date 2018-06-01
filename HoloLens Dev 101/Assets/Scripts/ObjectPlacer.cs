//---------------------
// Source code from https://www.cameronvetter.com/2017/01/30/hololens-tutorial-object-placement-and-scaling/
// Chris Darcy, June 2018
//
// --------------------

using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using System;

public class ObjectPlacer : MonoBehaviour {

    public SpatialUnderstandingCustomMesh SpatialUnderstandingMesh;
    private bool _timeToHideMesh;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (_timeToHideMesh)
        {
            SpatialUnderstandingState.Instance.HideText = true;
            HideGridEnableOcclusion();
            _timeToHideMesh = false;
        }
	}

    private void HideGridEnableOcclusion()
    {
        SpatialUnderstandingMesh.DrawProcessedMesh = false;
    }

    public void CreateScene()
    {
        if (!SpatialUnderstanding.Instance.AllowSpatialUnderstanding)
        {
            return;
        }
        _timeToHideMesh = true;
    }
}
