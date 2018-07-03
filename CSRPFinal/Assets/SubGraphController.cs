using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//using BulletUnity;

public class SubGraphController : MonoBehaviour
{
    // Storage unit for the nodes we have
    public List<GameObject> subNodes = new List<GameObject>(); // TODO: See if I need to delete this
    public List<Link> links = new List<Link>();

    [SerializeField]
    private static bool verbose = true;

    private static GameController gameControl;
    //private static GameCtrlUI gameCtrlUI;

    [SerializeField]
    private bool allStatic = false;
    [SerializeField]
    private bool paintMode = false;
    [SerializeField]
    private bool repulseActive = true;
    [SerializeField]
    private bool debugRepulse = false;

    // Prefabs
    public GameObject subNodePrefab;
    public Link linkPrefab;

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


    //public GameObject selectedNode = null;

    private static int subNodeCount;
    private static int subLinkCount;
    private List<GameObject> debugObjects = new List<GameObject>();

    // Collection of dummy variables for debug
    private DateTime debugTime = DateTime.Now;
    private int debugChildNodes = 3;
    private string host = "example.com";

    private GameObject parent;



    [SerializeField]
    public int LinkCount
    {
        get
        {
            return subLinkCount;
        }
        set
        {
            subLinkCount = value;
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
            return subNodeCount;
        }
        set
        {
            subNodeCount = value;
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

    private GameObject InstSubNode(Vector3 createPos, string text, bool hide = false)
    {

        return SubGraphNode.CreateInstance(subNodePrefab, parent, createPos, text, hide).getObject() as GameObject;

    }


    private GameObject InstSubNode(Vector3 createPos)
    {
        // Overload for debugging purposes
        return SubGraphNode.CreateInstance(subNodePrefab, parent, createPos, "ftp.example.com").getObject() as GameObject;
    }


    public GameObject GenSubNode()
    {
        // Method for creating a Node on random coordinates, e.g. when spawning multiple new nodes

        GameObject nodeCreated = null;

        Vector3 createPos = new Vector3(UnityEngine.Random.Range(0, nodeVectorGenRange), UnityEngine.Random.Range(0, nodeVectorGenRange), UnityEngine.Random.Range(0, nodeVectorGenRange));

        // TODO: Replace with live-data version
        nodeCreated = InstSubNode(createPos);


        if (nodeCreated != null)
        {
            nodeCreated.name = "node_" + subNodeCount;
            subNodeCount++;


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

    public GameObject GenSubNode(Vector3 createPos)
    {
        // Method for creating a Node on specific coordinates, e.g. in Paintmode when a node is created at the end of a paintedLink

        GameObject nodeCreated = null;

        nodeCreated = InstSubNode(createPos);

        if (nodeCreated != null)
        {
            nodeCreated.name = "node_" + subNodeCount;
            subNodeCount++;

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
        nodeCreated = InstSubNode(createPos);

        if (nodeCreated != null)
        {
            Node nodeNode = nodeCreated.GetComponent<Node>();
            nodeNode.name = id;
            nodeNode.Text = name;
            nodeNode.Type = type;

            subNodeCount++;


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

    public Link CreateSubLink(GameObject source, GameObject target)
    {
        print("Entering SubLink function for SGC-" + parent.name);
        if (source == null || target == null)
        {
            if (verbose)
            {
                Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": source or target does not exist. Link not created.");
            }
            return null;
        }
        else
        {
            if (source != target)
            {
                bool alreadyExists = false;
                foreach (Link checkObj in links)
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
                    linkObject.name = "sublink_" + subLinkCount;
                    linkObject.source = source;
                    linkObject.target = target;
                    subLinkCount++;

                    return linkObject;
                }
                else
                {
                    if (verbose)
                    {
                        Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Link between source " + source.name + " and target " + target.name + " already exists. Link not created.");
                    }
                    return null;
                }
            }
            else
            {
                if (verbose)
                {
                    Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": source " + source.name + " and target " + target.name + " are the same. Link not created.");
                }
                return null;
            }
        }
    }

    public void GenerateLink(string mode)
    {
        if (mode == "random")
        {
            bool success = false;
            int tryCounter = 0;
            int tryLimit = subNodeCount * 5;

            while (!success && tryCounter < tryLimit)
            {
                tryCounter++;

                int sourceRnd = UnityEngine.Random.Range(0, subNodeCount);
                int targetRnd = UnityEngine.Random.Range(0, subNodeCount);

                GameObject source = GameObject.Find("node_" + sourceRnd);
                GameObject target = GameObject.Find("node_" + targetRnd);

                success = CreateSubLink(source, target);
            }
            if (!success)
                if (verbose)
                    Debug.Log(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Too many unsuccessful tries, limit reached. Bailing out of GenerateLink run with mode=random. TryCounter: " + tryCounter + " Limit: " + subNodeCount * 5);
        }
    }

    public void GenerateLink(string mode, GameObject source, GameObject target)
    {
        // TODO: This overload does not need a string mode variable - one could do away with the parameter entirely.
        if (mode == "specific_src_tgt")
        {
            Link success = null;

            success = CreateSubLink(source, target);

            if (success == null)
                if (verbose)
                    Debug.LogWarning(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": Problem with creating link. Link not created.");
                else
                {

                    // Just for Debugging SubNode links
                    print("Created link between " + source.name + " and " + target.name);
                    links.Add(success);
                }

        }
    }

    public void GenNodes(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Create a node on random Coordinates
            GenSubNode();
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

        // remove the objects that have been subtracted from the graph since the last update
        foreach (GameObject subNode in NodesInChildren())
        {
            print("UpdateLinks looping through " + parent.name + "/" + subNode.name[subNode.name.Length - 1]);
            // Is the object flagged for deletion?
            NodePhysX nodeInfo = subNode.GetComponent<NodePhysX>();
            if (nodeInfo.delete)
            {
                print("Destroying " + subNode.name);
                subNodes.Remove(subNode);
                Destroy(subNode);

            }
            else if (nodeInfo.hide)
            {
                // Hide the mesh+collider
                print("Hiding " + subNode.name);
                subNode.SetActive(false);
                nodeInfo.hide = false;
            }

            else if (nodeInfo.unHide && subNode.activeSelf == false)
            {
                // Hide the mesh+collider
                print("Un-Hiding " + subNode.name);
                subNode.SetActive(true);
                nodeInfo.unHide = false;
                // re-create the link
                GenerateLink("specific_src_tgt", subNode, subNode.GetComponent<NodePhysX>().root);
            }
            else if (nodeInfo.isNew)
            {
                // A new link must be created
                GenerateLink("specific_src_tgt", subNode, subNode.GetComponent<NodePhysX>().root);
                nodeInfo.isNew = false;
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
        foreach (Link link in links)
        {

            // Two cases here: one for if the link has a null source (destroyed) and one for hidden nodes
            if (link.GetComponent<Link>().source == null || !link.GetComponent<Link>().source.activeSelf)
            {
                print("Scrubbing link " + link.name);
                links.Remove(link);
                Destroy(link);
                LinkCount -= 1;
            }

        }


    }

    void Start()
    {
        parent = transform.parent.gameObject;
        print("SGC has associated " + parent.name + " as its parent");
        subNodeCount = 0;
        subLinkCount = 0;

        foreach (GameObject obj in NodesInChildren())
        {
            // Create the initial links
            // TODO: Create requirements/warnings that the nodes have a NodePhysX component.
            if (obj == null)
            {
                continue;
            }
            GenerateLink("specific_src_tgt", obj, obj.GetComponent<NodePhysX>().root);
        }

    }

    // TODO: Implement this in GraphController
    private IEnumerable<GameObject> NodesInChildren()
    {
        foreach(Transform child in parent.transform)
        {
            if (!GetComponent<NodePhysX>()) { continue; }
            yield return transform.gameObject;
        }
        yield break;
    }

    private void NewSubNode()
    {
        ///<summary>This function creates a new host on random coordinates, as well as a link between it and the root.</summary>
        ///

        GameObject newNode = GenSubNode();

        subNodes.Add(newNode);
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
