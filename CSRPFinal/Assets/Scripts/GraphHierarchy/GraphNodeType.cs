using System;
using UnityEngine;

public class GraphNodeType : ScriptableObject {

    private GameObject thisObject;
    private DateTime nodeTime;
    private bool IsActive;
    private Vector3 position;
    private string hostIP;
    private string remoteAddress;
    private int fields;
    private SubGraphNode[] subNodes;

    // metadata for tanium objects
    private int process_id;
    private string process_name;
    private string username;
    
    public static GraphNodeType CreateInstance(GameObject go, DateTime time, string host, Vector3 pos, GameObject miniGo, bool act = false, int num = 3)
    {
        GraphNodeType inst = CreateInstance<GraphNodeType>();
        inst.Init(go, time, host, pos, act, num);
        return inst;
    }

    private void Init(GameObject go, DateTime time, string host, Vector3 pos, bool act, int num)
    {
        thisObject = go;
        nodeTime = time;
        IsActive = act;
        position = pos;
        hostIP = host;
        fields = num;

        // create base gameobject
        thisObject = Instantiate(thisObject, pos, Quaternion.identity);
        

    }

    // ************ Accessors *****************
    public GameObject getObject()
    {
        return thisObject;
    }

    public bool getActive()
    {
        return IsActive;
    }

    public DateTime getTime()
    {
        return nodeTime;
    }

    public Vector3 getPosition()
    {
        return position;
    }

    public string getHost()
    {
        return hostIP;
    }

    public int getFields()
    {
        return fields;
    }

    public SubGraphNode[] getSubGraphNodes()
    {
        return subNodes;
    }

    public string getRemoteAddress()
    {
        return remoteAddress;
    }

    // following methods for tanium data
    public int getProcessId()
    {
        return process_id;
    }

    public string getProcessName()
    {
        return process_name;
    }

    public string getProcessUser()
    {
        return username;
    }

    // ************ Mutators *****************
    public void setTime(DateTime newTime)
    {
        nodeTime = newTime;
    }

    public void setActive(bool newActive)
    {
        if (newActive != IsActive)
        {
            // change mesh if there is a change
            thisObject.GetComponent<Renderer>().enabled = newActive;
        }
        IsActive = newActive;
    }

    public void setObject(GameObject newObject)
    {
        thisObject = newObject;
    }

    public void setPosition(Vector3 newPos)
    {
        position = newPos;  // set position
        // now move object to that position
        thisObject.transform.position = position;
    }

    public void setHost(string newHost)
    {
        hostIP = newHost;
    }

    public void incrementFields(int optionalIncrement = 1)
    {
        fields += optionalIncrement;
    }

    public void InitSubNodes(string[] subs, GameObject go)
    {
        // it would be improper to call this function to add sub nodes
        // this is only for initial creation
        // JCL: but what if I hold my pinky up when I call it

        subNodes = new SubGraphNode[subs.Length];
        int i = 0;
        foreach (string str in subs)
        {
            SubGraphNode sgn = SubGraphNode.CreateInstance(go, thisObject, Vector3.zero, str);
            sgn.getObject().transform.parent = thisObject.transform;
            subNodes[i++] = sgn;
        }
        positionSubNodes();
    }

    public void positionSubNodes(float r = 0.05f)
    {
        double theta = (Math.PI / 2) / (fields - 1);
        double startPos = 0;
        
        for (int i = 0; i < subNodes.Length; i++)
        {
            if (this.position.x < 0)
            {
                startPos = Math.PI / 2;   // shift over to left side of nodes
                subNodes[i].getObject().GetComponent<TextMesh>().anchor = TextAnchor.UpperRight;   
            }
            else if(Mathf.Round(this.position.x * 1000f)/1000f == 0)
            {
                startPos = Math.PI / 4;
                subNodes[i].getObject().transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
            }
            // set up positioning 
            double angle = theta * i + startPos;                                    // offset angle
            float xPos = Convert.ToSingle(r * Math.Cos(angle)) + this.position.x;   // get X and convert back to float
            float yPos = Convert.ToSingle(r * Math.Sin(angle)) + this.position.y;   // for y

            subNodes[i].setPosition(new Vector3(xPos, yPos, this.position.z));
        }
    }

    public void ToggleActiveSubs()
    {
        foreach(SubGraphNode sgn in subNodes)
        {
            sgn.ToggleMesh();
        }
    }

    public void setRemoteAddress(string newIP)
    {
        remoteAddress = newIP;
    }

    // following methods for tanium processes

    public void setProcessId(int new_id)
    {
        process_id = new_id;
    }

    public void setProcessName(string new_name)
    {
        process_name = new_name;
    }

    public void setProcessUser(string new_user)
    {
        username = new_user;
    }

}
