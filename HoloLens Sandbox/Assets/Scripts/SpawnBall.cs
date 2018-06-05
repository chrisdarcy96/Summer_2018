﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using HoloToolkit.Unity.InputModule;

public class SpawnBall : MonoBehaviour, IInputClickHandler {

    public GameObject ballPrefab;

	// Initialize when script is loaded
	void Start() {
        // fallback input handler called when no object is focused to consume click
        InputManager.Instance.PushFallbackInputHandler(gameObject);
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("Click");
        Vector3 pos = Camera.main.transform.position;
        Vector3 dir = Camera.main.transform.forward;
        CreateBouncingBall(pos, dir);
    }

    private void CreateBouncingBall(Vector3 pos, Vector3 dir)
    {
        Vector3 position = new Vector3(pos.x + dir.x * 2.0f, 1.5f, pos.z + dir.z * 2.0f);
        Instantiate(ballPrefab, position, Quaternion.identity);
        Debug.Log("Ball placed at " + position);
    }
}
