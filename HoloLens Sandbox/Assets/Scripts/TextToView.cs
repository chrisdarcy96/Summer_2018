﻿using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

public class TextToView : MonoBehaviour {

    public string path = "Assets/httplistener.json";
    public List<Dictionary<string, string>> Connections;
    public GameObject Node_Prefab;


	// Use this for initialization
	void Start() {
        Connections = new List<Dictionary<string, string>>();
        

        // read JSON file on connections
        ReadJSON();
        print(PrettyPrint(Connections));

        // create game Objects
        GameObject[] nodes = CreateNodes();
	}


	
    private GameObject[] CreateNodes()
    {
        GameObject[] nodes = new GameObject[Connections.Count];
        int i = 0;
        foreach(Dictionary<string, string> pair in Connections)
        {   
            // more can be done here with data provide in Dictionary pairs
            GameObject newNode = Instantiate(Node_Prefab);
            nodes[i++] = newNode;
        }

        return nodes;
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
                print("here");
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

