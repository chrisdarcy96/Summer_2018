using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

//TODO: Potentially may want to rename this.
public class Interactible : MonoBehaviour, IFocusable {

    public Shader glow;
    
    

    public void OnFocusEnter()
    {
        GetComponent<Renderer>().material.shader = glow;
    }

    public void OnFocusExit()
    {
        GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
    }

    // Use this for initialization
    void Start () {

}
	
	// Update is called once per frame
	void Update () {
		
	}
}
