using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//using BulletUnity;

public class GraphController : MonoBehaviour {
    // Storage unit for the nodes we have
    public List<GameObject> nodes = new List<GameObject>();

    //// Bins for each time I need an action done on nodes. This is to avoid an iteration of nodes each frame.
    //private List<GameObject> needToHide = new List<GameObject>();
    //private List<GameObject> hidden = new List<GameObject>();
    //private List<GameObject> deathRow = new List<GameObject>();



    [SerializeField]
    private static bool verbose = true;

    private static GameController gameControl;
    //private static GameCtrlUI gameCtrlUI;
    private static GameCtrlHelper gameCtrlHelper;

    [SerializeField]
    private bool allStatic = false;
    [SerializeField]
    private bool paintMode = false;
    [SerializeField]
    private bool repulseActive = true;
    [SerializeField]
    private bool debugRepulse = false;

    [SerializeField]
    private GameObject nodePrefabBullet;
    [SerializeField]
    private GameObject nodePrefabPhysX;
    [SerializeField]
    private Link linkPrefab;
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
    private float linkIntendedLinkLength = 5F;

    private static int nodeCount;
    private static int linkCount;
    private List<GameObject> debugObjects = new List<GameObject>();

    [SerializeField]
    public int LinkCount
    {
        get
        {
            return linkCount;
        }
        set
        {
            linkCount = value;
        }
    }


    public bool AllStatic
    {
        get
        {
            return allStatic;
        }
        set
        {
            allStatic = value;
        }
    }

    public bool PaintMode
    {
        get
        {
            return paintMode;
        }
        set
        {
            paintMode = value;
        }
    }

    public bool RepulseActive
    {
        get
        {
            return repulseActive;
        }
        set
        {
            repulseActive = value;
        }
    }

    public bool DebugRepulse
    {
        get
        {
            return debugRepulse;
        }
        set
        {
            if (debugRepulse != value)
            {
                debugRepulse = value;
                DebugAllNodes();
            }
        }
    }

    public float GlobalGravityBullet
    {
        get
        {
            return globalGravityBullet;
        }
        private set
        {
            globalGravityBullet = value;
        }
    }

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



    void DebugAllNodes()
    {
        if (DebugRepulse)
        {
            foreach (GameObject debugObj in debugObjects)
            {
                debugObj.SetActive(true);
                if (debugObj.name == "debugRepulseObj")
                {
                    float sphereDiam = gameCtrlHelper.GetRepulseSphereDiam();
                    debugObj.transform.localScale = new Vector3(sphereDiam, sphereDiam, sphereDiam);
                }
            }
        }
        else
        {
            foreach (GameObject debugObj in debugObjects)
            {
                debugObj.SetActive(false);
            }
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

        debugObjects.Clear();
    }

    private GameObject InstObj(Vector3 createPos)
    {
        if (gameControl.EngineBulletUnity)
        {
            return Instantiate(nodePrefabBullet, createPos, Quaternion.identity) as GameObject;
        }
        else
        {
            return Instantiate(nodePrefabPhysX, createPos, Quaternion.identity) as GameObject;
        }
    }

    public GameObject GenerateNode()
    {
        // Method for creating a Node on random coordinates, e.g. when spawning multiple new nodes

        GameObject nodeCreated = null;

        Vector3 createPos = new Vector3(UnityEngine.Random.Range(0, nodeVectorGenRange), UnityEngine.Random.Range(0, nodeVectorGenRange), UnityEngine.Random.Range(0, nodeVectorGenRange));

        nodeCreated = InstObj(createPos);

        if (nodeCreated != null)
        {
            nodeCreated.name = "node_" + nodeCount;
            nodeCount++;
            //gameCtrlUI.PanelStatusNodeCountTxt.text = "Nodecount: " + NodeCount;

            GameObject debugObj = nodeCreated.transform.Find("debugRepulseObj").gameObject;
            debugObjects.Add(debugObj);
            debugObj.SetActive(false);

            if (verbose)
                Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Node created: " + nodeCreated.gameObject.name);

        } else
        {
            if (verbose)
                Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Something went wrong, did not get a Node Object returned.");
        }

        return nodeCreated.gameObject;
    }

    public GameObject GenerateNode(Vector3 createPos)
    {
        // Method for creating a Node on specific coordinates, e.g. in Paintmode when a node is created at the end of a paintedLink

        GameObject nodeCreated = null;

        //nodeCreated = Instantiate(nodePrefabBullet, createPos, Quaternion.identity) as Node;
        nodeCreated = InstObj(createPos);

        if (nodeCreated != null)
        {
            nodeCreated.name = "node_" + nodeCount;
            nodeCount++;
            //gameCtrlUI.PanelStatusNodeCountTxt.text = "Nodecount: " + NodeCount;

            GameObject debugObj = nodeCreated.transform.Find("debugRepulseObj").gameObject;
            debugObjects.Add(debugObj);
            debugObj.SetActive(false);

            if (verbose)
                Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Node created: " + nodeCreated.gameObject.name);
        }
        else
        {
            if (verbose)
                Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Something went wrong, did not get a Node Object returned.");
        }

        return nodeCreated.gameObject;
    }

    public GameObject GenerateNode(string name, string id, string type)
    {
        // Method for creating a Node on random coordinates, but with defined labels. E.g. when loaded from a file which contains these label.

        GameObject nodeCreated = null;

        Vector3 createPos = new Vector3(UnityEngine.Random.Range(0, nodeVectorGenRange), UnityEngine.Random.Range(0, nodeVectorGenRange), UnityEngine.Random.Range(0, nodeVectorGenRange));

        //nodeCreated = Instantiate(nodePrefabBullet, createPos, Quaternion.identity) as Node;
        nodeCreated = InstObj(createPos);

        if (nodeCreated != null)
        {
            Node nodeNode = nodeCreated.GetComponent<Node>();
            nodeNode.name = id;
            nodeNode.Text = name;
            nodeNode.Type = type;

            nodeCount++;
            //gameCtrlUI.PanelStatusNodeCountTxt.text = "Nodecount: " + NodeCount;

            GameObject debugObj = nodeCreated.transform.Find("debugRepulseObj").gameObject;
            debugObjects.Add(debugObj);
            debugObj.SetActive(false);

            if (verbose)
                Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Node created: " + nodeCreated.gameObject.name);
        }
        else
        {
            if (verbose)
                Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Something went wrong, no node created.");
        }

        return nodeCreated.gameObject;
    }

    public bool CreateLink(GameObject source, GameObject target)
    {
        if (source == null || target == null)
        {
            if (verbose)
            {
                Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": source or target does not exist. Link not created.");
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
                        Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Link between source " + source.name + " and target " + target.name + " already exists. Link not created.");
                    }
                    return false;
                }
            }
            else
            {
                if (verbose)
                {
                    Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": source " + source.name + " and target " + target.name + " are the same. Link not created.");
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
            if (!success)
                if (verbose)
                    Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Too many unsuccessful tries, limit reached. Bailing out of GenerateLink run with mode=random. TryCounter: " + tryCounter + " Limit: " + nodeCount * 5);
        }
    }

    public void GenerateLink(string mode, GameObject source, GameObject target)
    {
        if (mode == "specific_src_tgt")
        {
            bool success = false;

            success = CreateLink(source, target);

            if (!success)
                if (verbose)
                    Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Problem with creating link. Link not created.");
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
        
        foreach (GameObject node in nodes)
        {
            // Is the object flagged for deletion?
            NodePhysX nodeInfo = node.GetComponent<NodePhysX>();
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
                node.SetActive(false);
                nodeInfo.hide = false;
            }

            else if (nodeInfo.unHide && node.activeSelf == false)
            {
                // Hide the mesh+collider
                print("Un-Hiding " + node.name);
                node.SetActive(true);
                nodeInfo.unHide = false;
                // re-create the link
                GenerateLink("specific_src_tgt", node, node.GetComponent<NodePhysX>().root);
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
            if(link.GetComponent<Link>().source == null || !link.GetComponent<Link>().source.activeSelf)
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
        debugObjects.Clear();

        foreach (GameObject obj in nodes)
        {
            // Create the initial links
            // TODO: Create requirements/warnings that the nodes have a NodePhysX component.
            GenerateLink("specific_src_tgt", obj, obj.GetComponent<NodePhysX>().root);
        }

        // prepare stuff
        if (gameControl.EngineBulletUnity)
        {
            RepulseForceStrength = .1f;
            GlobalGravityBullet = 1f;
            LinkForceStrength = .1f;
            LinkIntendedLinkLength = 3f;
        } else
        {
            RepulseForceStrength = 5f;
            GlobalGravityPhysX = 10f;
            NodePhysXForceSphereRadius = 35f;
            LinkForceStrength = 5f;
            LinkIntendedLinkLength = 3f;
        }
        // Debug 
        for(int i = 0; i<10; i++)
        {
            NewHost();
        }
    }

    private void NewHost()
    {
        ///<summary>This function creates a new node on random coordinates, as well as a link between it and the root.</summary>
        ///

        GameObject newNode = GenerateNode();

        nodes.Add(newNode);
        GenerateLink("specific_src_tgt", newNode, newNode.GetComponent<NodePhysX>().root);

        print("Created new node named " + newNode.name);




    }

    void Update()
    {
        Link.intendedLinkLength = linkIntendedLinkLength;
        Link.forceStrength = linkForceStrength;
        UpdateLinks();
    }

}
