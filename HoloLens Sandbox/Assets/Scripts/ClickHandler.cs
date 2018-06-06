using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using HoloToolkit.Unity.InputModule;

public class ClickHandler : MonoBehaviour, IInputClickHandler
{

    public GameObject ballPrefab;

    // Initialize when script is loaded
    void Start()
    {
        // fallback input handler called when no object is focused to consume click
        InputManager.Instance.PushFallbackInputHandler(this.gameObject);
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (eventData.selectedObject.gameObject.CompareTag(ballPrefab.tag))
        {
            Destroy(eventData.selectedObject);
        }
        else
        {
            Vector3 pos = Camera.main.transform.position;
            Vector3 dir = Camera.main.transform.forward;
            CreateBouncingBall(pos, dir);
        }


    }

    private void CreateBouncingBall(Vector3 pos, Vector3 dir)
    {
        Vector3 position = pos + dir * 3.0f;
        // keep it above the ground
        if (position.y >= 0.0f)
        {
            position.y = 0.25f;
        }
        Instantiate(ballPrefab, position, Quaternion.identity);
    }
}

 