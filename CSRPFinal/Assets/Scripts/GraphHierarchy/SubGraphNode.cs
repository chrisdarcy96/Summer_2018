using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubGraphNode : ScriptableObject {

    private GameObject thisObject;
    private bool showMesh;
    private Vector3 position;
    private string data;

    public static SubGraphNode CreateInstance(GameObject go, GameObject parent, Vector3 pos, string text, bool showMesh = false)
    {
        SubGraphNode sgn = CreateInstance<SubGraphNode>();
        sgn.Init(go, parent, pos, text, showMesh);

        return sgn;
    }

    private void Init(GameObject go, GameObject parent, Vector3 pos, string text, bool showMesh)
    {
        thisObject = go;
        this.showMesh = showMesh;
        position = pos;
        data = text;

        thisObject = Instantiate(thisObject, position, Quaternion.identity);
        thisObject.gameObject.GetComponent<Renderer>().enabled = false;
        thisObject.transform.parent = parent.transform;
        thisObject.name = parent.name + "_SubGraphNode" + parent.transform.childCount;
        thisObject.GetComponent<TextMesh>().text = data;
    }


    // ************ Accessors *****************

    public GameObject getObject()
    {
        return thisObject;
    }

    public Vector3 getPosition()
    {
        return position;
    }

    public string getData()
    {
        return data;
    }

    public bool isMeshShown()
    {
        return showMesh;
    }

    // ************ Mutators *****************
    public void setPosition(Vector3 newPos)
    {
        position = newPos;
        thisObject.transform.position = position;
    }

    public void setData(string newText)
    {
        data = newText;
    }

    public void ToggleMesh()
    {
        showMesh = !showMesh;
        //Debug.Log("ToggleMesh called "+ showMesh);
        thisObject.GetComponent<Renderer>().enabled = showMesh; 

    }

    public void SetMesh()
    {
        showMesh = true;
        thisObject.GetComponent<Renderer>().enabled = showMesh;
    }

}
