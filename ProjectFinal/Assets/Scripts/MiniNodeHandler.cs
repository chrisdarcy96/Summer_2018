using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class MiniNodeHandler : MonoBehaviour, IFocusable
{
    private TextMesh examine;
    private TextMesh textGUI;
    
	// Use this for initialization
	void Start () {
        examine = this.gameObject.GetComponentInChildren<TextMesh>();
        examine.gameObject.SetActive(false);
        textGUI = GameObject.Find("Hinter").GetComponent<TextMesh>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnFocusEnter()
    {
        examine.gameObject.SetActive(true);
        textGUI.text = "Viewing: " + this.gameObject.tag + "_" + this.gameObject.GetInstanceID();
    }

    public void OnFocusExit()
    {
        examine.gameObject.SetActive(false);
        textGUI.text = "";
    }
}
