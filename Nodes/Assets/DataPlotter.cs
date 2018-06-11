using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPlotter : MonoBehaviour {

    // inputfile does NOT require .csv file extension
    [Tooltip("The name of the input file without the '.csv' extension.")]
    public string filename;

    private List<Dictionary<string, object>> points;


    // Use this for initialization
    void Start () {
        print("Hello, Nodes! I'm the DataPlotter Script!");

        // References CSVReader.cs, which returns a list compatible with our points variable.
        points = CSVReader.Read(filename);

        //Debug to console
        print(points);

        // Basic text representation of the CSV File

        

	}
	
}
