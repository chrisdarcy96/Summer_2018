using System;
using UnityEngine;

public class GraphNodeType : ScriptableObject {

    private GameObject thisObject;
    private SubGraphController childGraph;
    private DateTime nodeTime;
    private bool IsActive;
    private Vector3 position;
    private string hostIP;
    private int fields;
    private SubGraphNode[] subNodes;
    
    public static GraphNodeType CreateInstance(GameObject go, DateTime time, string host, Vector3 pos, GameObject miniGo, bool act = false, int num = 3)
    {
        GraphNodeType inst = CreateInstance<GraphNodeType>();
        inst.Init(go, time, host, pos, act, num);
        inst.InitSubNodes(new string[] {time.ToString(), host, pos.ToString() }, miniGo);
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

    public void positionSubNodes(float r = 0.1f)
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
            else if(Mathf.Round(this.position.x * 100f)/100f == 0)
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
            sgn.hide(!sgn.getHidden());
        }
    }

}
