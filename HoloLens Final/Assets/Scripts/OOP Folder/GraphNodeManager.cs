using System;
using UnityEngine;

public class GraphNodeManager : MonoBehaviour
{

    [SerializeField] private GraphNodeType[] graphNodes;
    [SerializeField] private bool hideAll = false;
    private bool oldHide;

    // Use this for initialization
    void Start()
    {
        graphNodes = FindObjectsOfType<GraphNodeType>();
        oldHide = !hideAll;
        SetAllActive();
    }

    private void SetAllActive()
    {
        foreach (GraphNodeType nod in graphNodes)
        {
            nod.setActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hideAll && oldHide != hideAll) { HideAllNodes(); }
        else if (!hideAll && oldHide != hideAll) { SetAllActive(); }
        oldHide = hideAll;
    }

    private void HideAllNodes()
    {
        foreach (GraphNodeType nod in graphNodes)
        {
            if (!nod.getActive())
            {
                Debug.LogWarning("Warning: Setting nodes to false that are already false");
            }
            else
            {
                nod.setActive(false);
            }

        }
        
    }
}
