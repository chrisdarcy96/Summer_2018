﻿using UnityEngine;
using System.Collections;

public class Link : MonoBehaviour
{
    public string id;
    public GameObject source;
    public GameObject target;
    public static float intendedLinkLength;
    public static float forceStrength;
    public float startWidth = .05f, endWidth = .05f;

    private static GameController gameControl;
    private static GraphController graphControl;

    private Component sourceRb;
    private Component targetRb;
    private LineRenderer lineRenderer;
    private float intendedLinkLengthSqr;
    private float distSqrNorm;


    void doAttraction()
    {
        if(sourceRb == null) { return; } // Should the source be null, this link only has frames to live - stall the forces for now.
        Vector3 forceDirection = sourceRb.transform.position - targetRb.transform.position;
        float distSqr = forceDirection.sqrMagnitude;

        if (distSqr > intendedLinkLengthSqr)
        {
            //Debug.Log("(Link.FixedUpdate) distSqr: " + distSqr + "/ intendedLinkLengthSqr: " + intendedLinkLengthSqr + " = distSqrNorm: " + distSqrNorm);
            distSqrNorm = distSqr / intendedLinkLengthSqr;

            Vector3 targetRbImpulse = forceDirection.normalized * forceStrength * distSqrNorm;
            Vector3 sourceRbImpulse = forceDirection.normalized * -1 * forceStrength * distSqrNorm;

                // Catch NaN forces
                // TODO: Figure out why there's some NaN forces right at the beginning of the simulation.
                if(float.IsNaN(sourceRbImpulse.x) && float.IsNaN(sourceRbImpulse.y) && float.IsNaN(sourceRbImpulse.z))
                {
                    //Debug.LogWarning("NaN force suppressed");
                    return;
                }
                //Debug.Log("(Link.FixedUpdate) targetRb: " + targetRb + ". forceDirection.normalized: " + forceDirection.normalized + ". distSqrNorm: " + distSqrNorm + ". Applying Impulse: " + targetRbImpulse);
                ((Rigidbody)targetRb as Rigidbody).AddForce(targetRbImpulse);
                //Debug.Log("(Link.FixedUpdate) targetRb: " + sourceRb + ". forceDirection.normalized: " + forceDirection.normalized + "  * -1 * distSqrNorm: " + distSqrNorm + ". Applying Impulse: " + sourceRbImpulse);
                ((Rigidbody)sourceRb as Rigidbody).AddForce(sourceRbImpulse);

        }
    }

    // Use this for initialization
    void Start()
    {
        gameControl = FindObjectOfType<GameController>();
        graphControl = FindObjectOfType<GraphController>();

        lineRenderer = gameObject.AddComponent<LineRenderer>();

        //color link according to status
        Color c;
        c = Color.gray;
        c.a = 0.5f;

        //draw line
        lineRenderer.material = new Material(Shader.Find("Self-Illumin/Diffuse"));
        lineRenderer.material.SetColor("_Color", c);
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = endWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, source.transform.position);
        lineRenderer.SetPosition(1, target.transform.position);

            sourceRb = source.GetComponent<Rigidbody>();
            targetRb = target.GetComponent<Rigidbody>();


        intendedLinkLengthSqr = intendedLinkLength * intendedLinkLength;
    }


    // Update is called once per frame
    void Update()
    {
        // moved from Start() in Update(), otherwise it won't see runtime updates of intendedLinkLength
        intendedLinkLengthSqr = intendedLinkLength * intendedLinkLength;

        lineRenderer.SetPosition(0, source.transform.position);
        lineRenderer.SetPosition(1, target.transform.position);
    }

    void FixedUpdate()
    {
        if (!graphControl.AllStatic)
            doAttraction();
    }
}
