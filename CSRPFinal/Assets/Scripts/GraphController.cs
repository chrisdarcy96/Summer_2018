using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//using BulletUnity;

public class GraphController : MonoBehaviour
{
    // Storage unit for the nodes we have
    [SerializeField]
    public List<GraphNodeType> nodes = new List<GraphNodeType>();

    [SerializeField]
    private static bool verbose = false;

    private static GameController gameControl;

    private bool repulseActive = true;
    [SerializeField]
    private bool debugRepulse = false;

    // Prefabs
    public GameObject hostPrefab;
    public GameObject procPrefab;
    public GameObject subNodePrefab;
    public Link linkPrefab;
    private bool allStatic;

    [SerializeField]
    private float nodeVectorGenRange = 7F;
    [SerializeField]
    private float globalGravityBullet = 0.1f;
    [SerializeField]
    private float globalGravityPhysX = 10f;
    [SerializeField]
    private float repulseForceStrength = 0.1f;
    [SerializeField]
    private float nodePhysXForceSphereRadius = 50F;                         // only works in PhysX; in BulletUnity CollisionObjects are used, which would need removing and readding to the world. Todo: Could implement it somewhen.
    [SerializeField]
    private float linkForceStrength = 6F;
    [SerializeField]
    private float linkIntendedLinkLength = 0.15F;
    [SerializeField, Tooltip("Adjust this slider to provide a scalar increase to the force that drives hosts upwards or downwards based on their tag"), Range(-2, 2)]
    public float stratificationScalingFactor = 1;
    [Tooltip("Use this to have the GraphController insert randomly placed hosts at runtime.")]
    public int randomNodes = 10;
    [Tooltip("If true, then the Random Nodes will be half host, half process.")]
    public bool hostsAndProcesses = true;

    //public GameObject selectedNode = null;

    private static int nodeCount;
    private static int linkCount;

    // Collection of dummy variables for debug
    private int debugChildNodes = 3;
    private string host = "example.com";

    [SerializeField]
    public int LinkCount { get; set; }
    public bool RepulseActive { get; set; }
    public bool AllStatic { get; set; }

    public float GlobalGravityPhysX
    {
        get
        {
            return globalGravityPhysX;
        }
        set
        {
            globalGravityPhysX = value;
        }
    }

    public float RepulseForceStrength
    {
        get
        {
            return repulseForceStrength;
        }
        private set
        {
            repulseForceStrength = value;
        }
    }

    public float NodePhysXForceSphereRadius
    {
        get
        {
            return nodePhysXForceSphereRadius;
        }
        set
        {
            nodePhysXForceSphereRadius = value;
        }
    }

    public float LinkForceStrength
    {
        get
        {
            return linkForceStrength;
        }
        private set
        {
            linkForceStrength = value;
        }
    }

    public float LinkIntendedLinkLength
    {
        get
        {
            return linkIntendedLinkLength;
        }
        set
        {
            linkIntendedLinkLength = value;
        }
    }

    public int NodeCount
    {
        get
        {
            return nodeCount;
        }
        set
        {
            nodeCount = value;
        }
    }

    public List<GraphNodeType> Nodes
    {
        get
        {
            return nodes;
        }
        set
        {
            nodes = value;
        }
    }


    public void ResetWorld()
    {
        foreach (GameObject destroyTarget in GameObject.FindGameObjectsWithTag("link"))
        {
            Destroy(destroyTarget);
            LinkCount -= 1;
            //gameCtrlUI.PanelStatusLinkCountTxt.text = "Linkcount: " + LinkCount;
        }

        foreach (GameObject destroyTarget in GameObject.FindGameObjectsWithTag("node"))
        {
            Destroy(destroyTarget);
            NodeCount -= 1;
            //gameCtrlUI.PanelStatusNodeCountTxt.text = "Nodecount: " + NodeCount;
        }

        foreach (GameObject destroyTarget in GameObject.FindGameObjectsWithTag("debug"))
        {
            Destroy(destroyTarget);
        }

    }

    public GraphNodeType InstHost(Vector3 createPos, DateTime metaTime, string hostname)
    {

        return GraphNodeType.CreateInstance(hostPrefab, metaTime, hostname, createPos, subNodePrefab);

    }


    public GraphNodeType InstProc(Vector3 createPos, DateTime metaTime, string hostname)
    {

        return GraphNodeType.CreateInstance(procPrefab, metaTime, hostname, createPos, subNodePrefab);

    }


    private GraphNodeType InstHost(Vector3 createPos)
    {
        // Overload for debugging purposes
        return GraphNodeType.CreateInstance(hostPrefab, DateTime.Now, "example.com", createPos, subNodePrefab);
    }

    private GraphNodeType InstProc(Vector3 createPos)
    {
        // debug overload
        return GraphNodeType.CreateInstance(procPrefab, DateTime.Now, "example.com", createPos, subNodePrefab);
    }


    public GraphNodeType GenerateNode(bool createProcess = false)
    {
        // Method for creating a Node on random coordinates, e.g. when spawning multiple new nodes

        GraphNodeType nodeCreated = null;

        Vector3 createPos = new Vector3(UnityEngine.Random.Range(0, nodeVectorGenRange), UnityEngine.Random.Range(0, nodeVectorGenRange), UnityEngine.Random.Range(0, nodeVectorGenRange));

        if (createProcess)
        {
            nodeCreated = InstProc(createPos);
        }
        else
        {
            nodeCreated = InstHost(createPos);
        }

        if (nodeCreated != null)
        {
            nodeCreated.name = "node_" + nodeCount;
            nodeCount++;


           // if (verbose)
               // Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Node created: " + nodeCreated.name);

        }
        else
        {
           // if (verbose)
               // Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Something went wrong, did not get a Node Object returned.");
        }

        return nodeCreated;
    }

    public GraphNodeType GenerateNode(Vector3 createPos)
    {
        // Method for creating a Node on specific coordinates, e.g. in Paintmode when a node is created at the end of a paintedLink
        // Debug overload that uses default information for the node's subnodes.
        GraphNodeType nodeCreated = null;

        nodeCreated = InstHost(createPos);

        if (nodeCreated != null)
        {
            nodeCreated.name = "node_" + nodeCount;
            nodeCount++;

           // if (verbose)
                //Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Node created: " + nodeCreated.name);
        }
        else
        {
          //  if (verbose)
                //Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Something went wrong, did not get a Node Object returned.");
        }

        return nodeCreated;
    }

    public GraphNodeType GenerateNode(Vector3 createPos, string hostname, DateTime metaTime)
    {
        // Method for creating a Node on specific coordinates, e.g. in Paintmode when a node is created at the end of a paintedLink
        // "Standard" overload for manual input of meta information
        GraphNodeType nodeCreated = null;

        nodeCreated = InstHost(createPos, metaTime, hostname);

        if (nodeCreated != null)
        {
            nodeCreated.name = "node_" + nodeCount;
            nodeCount++;

           // if (verbose)
              //  Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Node created: " + nodeCreated.name);
        }
        else
        {
           // if (verbose)
            //    Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Something went wrong, did not get a Node Object returned.");
        }

        return nodeCreated;
    }


    public bool CreateLink(GameObject source, GameObject target)
    {
        if (source == null || target == null)
        {
            if (verbose)
            {
                //Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": source or target does not exist. Link not created.");
            }
            return false;
        }
        else
        {
            if (source != target)
            {
                bool alreadyExists = false;
                foreach (GameObject checkObj in GameObject.FindGameObjectsWithTag("link"))
                {
                    Link checkLink = checkObj.GetComponent<Link>();
                    if (checkLink.source == source && checkLink.target == target)
                    {
                        alreadyExists = true;
                        break;
                    }
                }

                if (!alreadyExists)
                {
                    Link linkObject = Instantiate(linkPrefab, new Vector3(0, 0, 0), Quaternion.identity) as Link;
                    linkObject.name = "link_" + linkCount;
                    linkObject.source = source;
                    linkObject.target = target;
                    linkCount++;

                    return true;
                }
                else
                {
                    if (verbose)
                    {
                       // Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Link between source " + source.name + " and target " + target.name + " already exists. Link not created.");
                    }
                    return false;
                }
            }
            else
            {
                if (verbose)
                {
                   // Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": source " + source.name + " and target " + target.name + " are the same. Link not created.");
                }
                return false;
            }
        }
    }

    public void GenerateLink(string mode)
    {
        if (mode == "random")
        {
            bool success = false;
            int tryCounter = 0;
            int tryLimit = nodeCount * 5;

            while (!success && tryCounter < tryLimit)
            {
                tryCounter++;

                int sourceRnd = UnityEngine.Random.Range(0, nodeCount);
                int targetRnd = UnityEngine.Random.Range(0, nodeCount);

                GameObject source = GameObject.Find("node_" + sourceRnd);
                GameObject target = GameObject.Find("node_" + targetRnd);

                success = CreateLink(source, target);
            }
         //   if (!success)
                //if (verbose)
                  //  Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Too many unsuccessful tries, limit reached. Bailing out of GenerateLink run with mode=random. TryCounter: " + tryCounter + " Limit: " + nodeCount * 5);
        }
    }

    public void GenerateLink(string mode, GameObject source, GameObject target)
    {
        // TODO: This overload does not need a string mode variable - one could do away with the parameter entirely.
        if (mode == "specific_src_tgt")
        {
            bool success = false;

            success = CreateLink(source, target);

           // if (!success)
              //  if (verbose)
                  //  Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Problem with creating link. Link not created.");
        }
    }

    public void GenNodes(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Create a node on random Coordinates
            GenerateNode();
        }
    }

    public void GenLinks(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Create a link on random Coordinates
            GenerateLink("random");
        }
    }

    public void UpdateLinks()
    {
        ///<summary>
        /// The purpose of this function is to add, destroy, and hide links as necessary by iterating through the nodes list and checking the status of each node.
        /// Thought about creating a bunch of "status lists", but that would simply result in a ton of linear searches anyway.
        /// </summary>

        foreach (GraphNodeType node in nodes)
        {
            GameObject nodeObj = node.getObject();
            // Is the object flagged for deletion?
            NodePhysX nodeInfo = nodeObj.GetComponent<NodePhysX>();
            if (nodeInfo.delete)
            {
                print("Destroying " + node.name);
                nodes.Remove(node);
                Destroy(node);

            }
            else if (nodeInfo.hide)
            {
                // Hide the mesh+collider
                print("Hiding " + node.name);
                nodeObj.SetActive(false);
                nodeInfo.hide = false;
            }

            else if (nodeInfo.unHide && nodeObj.activeSelf == false)
            {
                // Hide the mesh+collider
                print("Un-Hiding " + node.name);
                nodeObj.SetActive(true);
                nodeInfo.unHide = false;
                // re-create the link
                GenerateLink("specific_src_tgt", nodeObj, nodeInfo.root);
            }
        }
        // After we're all done, remove the links.
        ScrubLinks();
    }

    private void ScrubLinks()
    {
        ///<summary>
        /// Runs through the links and removes any that are connected to hidden or null nodes.
        /// </summary>

        // Borrowing the find-and-iterate from ResetWorld
        foreach (GameObject link in GameObject.FindGameObjectsWithTag("link"))
        {

            // Two cases here: one for if the link has a null source (destroyed) and one for hidden nodes
            if (link.GetComponent<Link>().source == null || !link.GetComponent<Link>().source.activeSelf)
            {
                print("Scrubbing link " + link.name);
                Destroy(link);
                LinkCount -= 1;
                //gameCtrlUI.PanelStatusLinkCountTxt.text = "Linkcount: " + LinkCount;
            }

        }


    }

    void Start()
    {
        gameControl = GetComponent<GameController>();
        //gameCtrlUI = GetComponent<GameCtrlUI>();
        //gameCtrlHelper = GetComponent<GameCtrlHelper>();

        nodeCount = 0;
        linkCount = 0;

        foreach (GraphNodeType gnt in nodes)
        {
            // Create the initial links
            // TODO: Create requirements/warnings that the nodes have a NodePhysX component.\
            GameObject nodeObj = gnt.getObject();
            if (nodeObj == null)
            {
                continue;
            }
            GenerateLink("specific_src_tgt", nodeObj, nodeObj.GetComponent<NodePhysX>().root);
        }

        // Debug 
        for (int i = 0; i < randomNodes; i++)
        {
            if (hostsAndProcesses && i % 2 == 0)
            {
                NewProc();
            }
            else
            {
                NewHost();
            }
        }
    }

    private void NewHost()
    {
        ///<summary>This function creates a new host on random coordinates, as well as a link between it and the root.</summary>
        ///
        // Debug overload
        GraphNodeType newNode = GenerateNode();
        GameObject nodeObj = newNode.getObject();

        nodes.Add(newNode);
        GenerateLink("specific_src_tgt", nodeObj, nodeObj.GetComponent<NodePhysX>().root);

        //print("Created new node named " + newNode.name);

    }

    public void NewHost(Vector3 createPos, string hostname, DateTime metaTime)
    {
        ///<summary>This function creates a new host on set coordinates, as well as a link between it and the root.</summary>
        ///
        // "Standard" overload
        GraphNodeType newNode = GenerateNode(createPos, hostname, metaTime);
        GameObject nodeObj = newNode.getObject();
        nodes.Add(newNode);

        // Hierarchy maintenance - make this new node a child of the GraphController
        nodeObj.transform.parent = this.transform;

        GenerateLink("specific_src_tgt", nodeObj, nodeObj.GetComponent<NodePhysX>().root);

        //print("Created new host named " + newNode.name);

    }

    private void NewProc()
    {
        ///<summary>This function creates a new process on random coordinates, as well as a link between it and the root.</summary>
        ///
        //Debug overload

        GraphNodeType newNode = GenerateNode(createProcess: true);
        GameObject nodeObj = newNode.getObject();
        nodes.Add(newNode);
        GenerateLink("specific_src_tgt", nodeObj, nodeObj.GetComponent<NodePhysX>().root);

        //print("Created new process named " + newNode.name);

    }

    public void ToggleSubNodes(GameObject node)
    {
        ///<summary>Finds the provided GameObject in the list of nodes, then sets its GNT subnodes active or inactive based on the current state</summary>
        ///
        foreach (GraphNodeType gnt in nodes)
        {
            if (gnt.getObject() == node)
            {
                gnt.ToggleActiveSubs();
                break;
            }
        }

    }

    void Update()
    {
        Link.intendedLinkLength = linkIntendedLinkLength;
        Link.forceStrength = linkForceStrength;
        UpdateLinks();
    }

}
