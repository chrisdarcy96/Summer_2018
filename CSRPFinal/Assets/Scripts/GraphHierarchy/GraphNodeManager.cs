using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using HoloToolkit.Unity.SpatialMapping;
using System;
using HoloToolkit.Unity.InputModule;

public class GraphNodeManager : MonoBehaviour 
{

    public GameObject spawn;

    [SerializeField] private static Dictionary<GraphNodeType, SubGraphNode[]> masterNodes;
    [SerializeField] private bool hideAll = false;


    private bool oldHide;
    private KeywordRecognizer wordRecognizer;
    private Dictionary<string, System.Action> keywords;


    // Use this for initialization
    void Start()
    {
        // set up nodes
        masterNodes = new Dictionary<GraphNodeType, SubGraphNode[]>();
        GraphNodeType[] graphNodes = FindObjectsOfType<GraphNodeType>();
        foreach (GraphNodeType gnt in graphNodes)
        {
            masterNodes.Add(gnt, gnt.getSubGraphNodes());
        }
        oldHide = !hideAll;
        SetAllActive();

        // set up speech listeners
        keywords = new Dictionary<string, System.Action>
        {
            {
                "tap to place",
                () =>
                    {
                        // toggle the tap to place components
                        foreach (GraphNodeType gnt in masterNodes.Keys)
                        {
                            gnt.getObject().GetComponent<TapToPlace>().enabled = !gnt.getObject().GetComponent<TapToPlace>().enabled;
                        }
                    }
            }
        };
        wordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        wordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        wordRecognizer.Start();

    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Action keywordAction;
        if(keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
        Debug.Log("Recognized: " + args.text);
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
