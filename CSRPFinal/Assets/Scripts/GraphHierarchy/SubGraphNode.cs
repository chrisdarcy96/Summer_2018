using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubGraphNode : ScriptableObject {

    private GameObject thisObject;
    private bool hidden;
    private Vector3 position;
    private string data;

    public static SubGraphNode CreateInstance(GameObject go, Vector3 pos, string text, bool hide = false)
    {
        SubGraphNode sgn = CreateInstance<SubGraphNode>();
        sgn.Init(go, pos, text, hide);

        return sgn;
    }

    private void Init(GameObject go, Vector3 pos, string text, bool hide)
    {
        thisObject = go;
        hidden = hide;
        position = pos;
        data = text;

        thisObject = Instantiate(thisObject, position, Quaternion.identity);
        thisObject.gameObject.GetComponent<Renderer>().enabled = !hide;
        thisObject.name = "SubGraphNode";
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

    public bool getHidden()
    {
        return hidden;
    }

    // ************ Accessors *****************
    public void setPosition(Vector3 newPos)
    {
        position = newPos;
        thisObject.transform.position = position;
    }

    public void setData(string newText)
    {
        data = newText;
    }

    public void hide(bool hide = false)
    {
        hidden = hide;
        thisObject.GetComponent<Renderer>().enabled = !hidden;  //hidden = true, logically must be false to hide mesh randerer

    }
}
