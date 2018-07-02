using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

public class TextToView : MonoBehaviour {

    public string path = "Assets/httplistener.json";

    public GameObject Node_Prefab;
    public GameObject Parent_Object;

    private static List<Dictionary<string, string>> Connections;
    private static GameObject[] nodes;

    public static List<Dictionary<string, string>> GetConnections { get { return Connections; } }
    public static GameObject[] Nodes { get { return nodes; } }


	// Use this for initialization
	void Start() {
        Connections = new List<Dictionary<string, string>>();

        // read JSON file on connections
        ReadJSON();
        print(PrettyPrint(Connections));

        // create game Objects
        nodes = CreateNodes();
	}


	
    private GameObject[] CreateNodes()
    {
        GameObject[] node = new GameObject[Connections.Count];
        int i = 0;
        foreach(Dictionary<string, string> pair in Connections)
        {   
            // more can be done here with data provide in Dictionary pairs
            GameObject newNode = Instantiate(Node_Prefab);

            // make new nodes children of ParentObject (should be NodeManager game object)
            newNode.transform.parent = Parent_Object.transform;
            newNode.name = "node-"+i;

            // hide this node from view
            // newNode.SetActive(false);
            float x;
            float y;
            GetPoints(i, out x, out y);
            //Debug.Log("moving to: " + x + ", " + y);
            newNode.transform.position = new Vector3(x, y, 2);
            node[i++] = newNode;
        }
        return node;
    }

    private void GetPoints(int i, out float xPos, out float yPos)
    {
        // split 360 degress (or 2pi) into equal fractions
        double theta = (Math.PI) / (Connections.Count-1);
        double angle = theta * i;   // angle moves this around the circle

        xPos = Convert.ToSingle(.25 * Math.Cos(angle));  // get X and convert back to float
        yPos = Convert.ToSingle(.25 * Math.Sin(angle));  // for y
    } 

    private void ReadJSON()
    {

        using (StreamReader sr = new StreamReader(File.OpenRead(path)))
        {
            //string json_item = sr.ReadToEnd();  // reads entire JSON file into String (there's no way this will ever break...)
            string json_item = sr.ReadLine();   // read single line into string
            while(json_item != null && json_item != "")
            {
                Dictionary<string, string> fields = new Dictionary<string, string>();
                // found https://stackoverflow.com/questions/13297563/read-and-parse-a-json-file-in-c-sharp
                // probably look into this more in depth
                JObject line = JsonConvert.DeserializeObject<JObject>(json_item);
                // https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_Linq_JObject.htm
                

                JProperty result = line.Property("result"); // get "result" property
                // https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_Linq_JProperty.htm

                JToken actual = result.First;   // returns first JToken in JProperty (there should only be one in this case)
                                                // https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_Linq_JToken.htm


                // now add each token in the JToken to Dictionary
                IEnumerable<JToken> childs = actual.Children();
                foreach(JToken child in childs)
                {
                    string[] split = child.ToString().Split(new char[] { ':' }, 2);

                    split[0] = split[0].Trim().Trim('"');   // kill leading whitespace and " char
                    split[1] = split[1].Trim().Trim('"');

                    fields.Add(split[0], split[1]);
                }

                // Add this result to list
                Connections.Add(fields);

                json_item = sr.ReadLine();
            }

        }
    }

    // this is unnecessary, but makes testing/debugging a whole lot easier
    private string PrettyPrint(List<Dictionary<string, string>> list)
    {
        int conn_num = 1;
        StringBuilder sb = new StringBuilder();
        foreach(Dictionary<string, string> connection in list)
        {
            StringBuilder conn = new StringBuilder();
            conn.Append("*** Connection: ");
            conn.Append(conn_num++);
            conn.AppendLine();
            
            foreach(string key in connection.Keys)
            { 
                conn.Append('\t');
                string value = "";
                connection.TryGetValue(key, out value);
                conn.AppendLine(key +": "+ value);
            }
            sb.AppendLine(conn.ToString());
            conn.Clear();

        }
        return sb.ToString();
    }
}

