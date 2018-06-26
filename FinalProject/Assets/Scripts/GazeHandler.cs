using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using HoloToolkit.Unity.InputModule;
using UnityEngine.UI;


public class GazeHandler : MonoBehaviour, IFocusable
{ 
    private TextMesh info;
    private Vector3 velocity_vector;
    private Vector3 angVelocity_vector;

    // Use this for initialization
    void Start()
    {
        // set up text mesh
        info = GetComponentInChildren<TextMesh>();
        info.transform.position = this.gameObject.transform.position;
        info.gameObject.SetActive(false);
        

    }
    void Update()
    {
       
    }

    public void OnFocusEnter()
    {   
        // stop ball and face it towards camera
        velocity_vector = this.gameObject.GetComponent<Rigidbody>().velocity;
        this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        angVelocity_vector = this.gameObject.GetComponent<Rigidbody>().angularVelocity;
        this.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);

        // display info
        info.gameObject.SetActive(true);
        string examine = "Tag: " + gameObject.tag + "\nPosition: " + this.gameObject.transform.position + "\n";
        info.text = examine;

        // ensure text is facing Camera always
        // info.transform.LookAt(Camera.main.transform.forward);
    }

    public void OnFocusExit()
    {
        // start moving ball again
        this.gameObject.GetComponent<Rigidbody>().velocity = velocity_vector;
        this.gameObject.GetComponent<Rigidbody>().angularVelocity = angVelocity_vector;
        // hide text
        info.gameObject.SetActive(false);
    }
}
