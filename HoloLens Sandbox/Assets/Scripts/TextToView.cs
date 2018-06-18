using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TextToView : MonoBehaviour {

    public string path = "Assets/info.txt";

	// Use this for initialization
	void Start () {
        ReadFile();
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
}
