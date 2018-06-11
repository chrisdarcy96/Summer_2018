using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Props to the awesome people from Penn State at https://sites.psu.edu/bdssblog/2017/04/06/basic-data-visualization-in-unity-scatterplot-creation/

public class DataPlotter : MonoBehaviour {

    // inputfile does NOT require .csv file extension
    [Tooltip("The name of the input file without the '.csv' extension.")]
    public string filename;
    [Tooltip("A Prefab of the object we will populate the graph with.")]
    public GameObject ptprefab;

    private List<Dictionary<string, object>> points;
    
    // Indices for columns to be assigned
    public int columnX = 0; // No name will populate in xName by default because in the .csv is the input column.
    public int columnY = 1;
    public int columnZ = 2;

    // Full column names. Placeholders until after the data is read.
    public string xName;
    public string yName;
    public string zName;

    // Use this for initialization
    void Start () {

        // Pull in the Nexus from DrawCylinder and then the nodes list from the Nexus
        GameObject nexus = GameObject.Find("LineNexus");
        List<GameObject> nodes = nexus.GetComponent<DrawCylinders>().nodes;

        // Debug checks and heartbeat.
        if (nexus == null)
        {
            print("Nexus not found!");
        }

        print("Hello, Nodes! I'm the DataPlotter Script!");

        // References CSVReader.cs, which returns a list compatible with our points variable.
        points = CSVReader.Read(filename);

        //Debug to console
        print(points);

        // Basic text representation of the CSV File
        // Declare list of strings, fill with keys (column names)
        List<string> columnList = new List<string>(points[1].Keys);

        // Print number of keys (using .count)
        print("There are " + columnList.Count + " columns in CSV");

        foreach (string key in columnList)
        print("Column name is " + key);

        // Assign column name from columnList to Name variables
        xName = columnList[columnX];
        yName = columnList[columnY];
        zName = columnList[columnZ];

        // Loop through the points and plot the graph
        // Offset the Z +2 because the HoloLense camera is at the true origin.

        for (int i = 0; i < points.Count; i++)
        {
            // Find each representative value
            float x = System.Convert.ToSingle(points[i][xName]); // System.Convert.ToSingle just ensures that what we're using is a float
            float y = System.Convert.ToSingle(points[i][yName]);
            float z = System.Convert.ToSingle(points[i][zName]) + 2.0f;

            GameObject temp = Instantiate(ptprefab, new Vector3(x, y, z), Quaternion.identity);
            temp.name = "PlotData" + i;
            temp.transform.parent = nexus.transform;
            // Try to tack these clones into the Nexus
            nodes.Add(temp);
        }

    }
	
}
