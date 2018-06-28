using System;
using System.Collections.Generic;
using UnityEngine;

public class GraphNodeManager : MonoBehaviour
{

    public GameObject spawn;

    [SerializeField] private static Dictionary<GraphNodeType, SubGraphNode[]> masterNodes;
    [SerializeField] private bool hideAll = false;
    private bool oldHide;

    // Use this for initialization
    void Start()
    {
        masterNodes = new Dictionary<GraphNodeType, SubGraphNode[]>();
        GraphNodeType[] graphNodes = FindObjectsOfType<GraphNodeType>();
        foreach(GraphNodeType gnt in graphNodes)
        {
            masterNodes.Add(gnt, gnt.getSubGraphNodes());
        }
        oldHide = !hideAll;
        SetAllActive();
    }


    private void SetAllActive()
    {
        foreach(GraphNodeType key in masterNodes.Keys)
        {
            foreach(SubGraphNode sgn in masterNodes[key])
            {
                sgn.hide(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hideAll && oldHide != hideAll) {  }
        else if (!hideAll && oldHide != hideAll) { SetAllActive(); }
        oldHide = hideAll;
    }

    public static void ToggleSubNodes(GameObject focused)
    {
        foreach(GraphNodeType gnt in masterNodes.Keys)
        {
            if(gnt.getObject().GetInstanceID() == focused.GetInstanceID())
            {
                // if they are same game object
                // turn on sub nodes
                gnt.ToggleActiveSubs();
            }
        }
    }
}
