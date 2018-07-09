using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;


public class TaniumReader : MonoBehaviour {

    public string path = "taniumreader.json";

    public GraphController GraphManager; 
    private static GameObject[] nodes;

    public static List<Dictionary<string, string>> GetConnections { get; private set; }
    public static GameObject[] Nodes { get { return nodes; } }



    // Use this for initialization
    void Start() {
        GetConnections = new List<Dictionary<string, string>>();
        if (GraphManager == null)
        {
            GraphManager = GameObject.Find("GraphManager").GetComponent<GraphController>();
            if(GraphManager == null)
            {
                Debug.LogError("GraphManger not provided or found!");
            }
        }
        

        // read JSON file on connections
        ReadJSON();
        print(PrettyPrint(GetConnections));

        // create game Objects
        CreateNodes();
	}


	
    private void CreateNodes()
    {
        int i = 0;
        foreach(Dictionary<string, string> pair in GetConnections)
        {   
            // get neat splunk data
            DateTime time;
            string process_name;
            string user;
            int pid;
            GetUsefulInfo(pair, out process_name, out user, out pid);
            float x;
            float y;
            GetPoints(i++, out x, out y);
            GraphManager.NewProc(new Vector3(x,y,2), user, process_name, pid);

        }
    }

    private void GetPoints(int i, out float xPos, out float yPos)
    {
        // split 360 degress (or 2pi) into equal fractions
        double theta = (Math.PI) / (GetConnections.Count-1);
        double angle = (theta * i)+Math.PI;   // angle moves this around the circle

        xPos = Convert.ToSingle(.25 * Math.Cos(angle));  // get X and convert back to float
        yPos = Convert.ToSingle(.25 * Math.Sin(angle));  // for y
    } 

    private void ReadJSON()
    {
        // gets the file path that can be used on release
        string realFilePath = Path.Combine(Application.streamingAssetsPath, path);
        using (StreamReader sr = new StreamReader(File.OpenRead(realFilePath)))
        {
            //string json_item = sr.ReadToEnd();  // reads entire JSON file into String (there's no way this will ever break...)
            string json_item = sr.ReadLine();   // read single line into string

            while(json_item != null && json_item != "")
            {
                JArray arr = JArray.Parse(json_item);

                //print(arr.ToString());
                foreach(JObject obj in arr)
                {
                    Dictionary<string, string> fields = new Dictionary<string, string>();
                    foreach (KeyValuePair<string, JToken> pair in obj)
                    {
                        fields.Add(pair.Key, pair.Value.ToString());
                    }
                    
                    GetConnections.Add(fields);
                }

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
            conn.Append("*** Process: ");
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

    private void GetUsefulInfo(Dictionary<string, string> pair, out string process_name, out string host, out int pid)
    {
        string unformatted_time;
        pair.TryGetValue("create_time", out unformatted_time);
        string unformatted_pid;
        pair.TryGetValue("process_id", out unformatted_pid);

        process_name = pair["process_name"];
        host = pair["username"];
        pid = Int32.Parse(unformatted_pid);
    }

}

