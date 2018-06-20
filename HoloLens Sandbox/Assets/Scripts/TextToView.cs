using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class TextToView : MonoBehaviour {

    public string path = "Assets/httplistener.json";
    public List<Dictionary<string, string>> Connections;

    private Dictionary<string, string> fields;

	// Use this for initialization
	void Start () {
        Connections = new List<Dictionary<string, string>>();
        fields = new Dictionary<string, string>();

        //ReadFile();
        ReadJSON();
	}
	


    
    void ReadFile()
    {
        using (StreamReader sr = new StreamReader(File.OpenRead(path)))
        {
            string line = sr.ReadLine();
            while(line != null)
            {
                print(line);
                line = sr.ReadLine();
            }
        }
}

    void ReadJSON()
    {
        using (StreamReader sr = new StreamReader(File.OpenRead(path)))
        {
            //string json_item = sr.ReadToEnd();  // reads entire JSON file into String (there's no way this will ever break...)
            string json_item = sr.ReadLine();   // read single line into string
            while(json_item != null && json_item != "")
            {
                // found https://stackoverflow.com/questions/13297563/read-and-parse-a-json-file-in-c-sharp
                // probably look into this more in depth
                Newtonsoft.Json.Linq.JObject line = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json_item);
                
                // only work with the result field
                foreach(var comma in line)
                { 
                       
                    if(comma.Key == "result")
                    {
                        // EX: values are JToken in form: {"host" : "127.0.0.1","index":"main", etc...}
                   
                        string formatting = comma.Value.ToString();
                        formatting = formatting.Substring(1, formatting.Length - 2);    // strips first and last characters { and }
                        print(formatting);
                        string[] values = formatting.Split(',');

                        // place each field into dictionary
                        foreach(string index in values)
                        {
                            print(index);
                            // EX: index is in form: "host" : "127.0.0.1"
                            string[] pair = new string[2]; 
                            pair = index.Split(':');
                        }
                        
                        
                    }
                }
                
                // Add this result to list, clear dictionary for next line
                Connections.Add(fields);
                fields.Clear();
                json_item = sr.ReadLine();
            }

        }
    }
}

