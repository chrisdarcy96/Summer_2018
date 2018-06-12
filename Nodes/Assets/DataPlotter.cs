using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Props to the awesome people from Penn State at https://sites.psu.edu/bdssblog/2017/04/06/basic-data-visualization-in-unity-scatterplot-creation/

public class DataPlotter : MonoBehaviour
{

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
    void Start()
    {

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

        //Find the maxes and mins for normalization
        float xMax = FindMaxValue(xName);
        float yMax = FindMaxValue(yName);
        float zMax = FindMaxValue(zName);

        float xMin = FindMinValue(xName);
        float yMin = FindMinValue(yName);
        float zMin = FindMinValue(zName);
        // Loop through the points and plot the graph

        for (int i = 0; i < points.Count; i++)
        {
            // Find each representative value
            float x = (Convert.ToSingle(points[i][xName]) - xMin) / (xMax-xMin); // System.Convert.ToSingle just ensures that what we're using is a float
            float y = (Convert.ToSingle(points[i][yName]) - yMin) / (yMax - yMin);
            float z = (Convert.ToSingle(points[i][zName]) - zMin) / (yMax - yMin) + 2.0f; // Offset the Z +2 because the HoloLense camera is at the true origin.

            GameObject temp = Instantiate(ptprefab, new Vector3(x, y, z), Quaternion.identity);
            temp.name = "PlotData" + i;
            temp.transform.parent = nexus.transform;
            // Try to tack these clones into the Nexus
            nodes.Add(temp);
        }

    }

    private float FindMaxValue(string columnName)
    {
        ///<summary>
        /// A Function that takes a columnName as an argument and returns the maximum value in that column.
        /// Only compatible with the points List format - specifically, the one from  the Penn State reference above.
        /// </summary>
        /// <param name="columnName">
        /// A string that matches with a column in the points list of dictionaries.
        /// </param>

        // Whatever is first will be our max by default
        float max = Convert.ToSingle(points[0][columnName]);

        // Loop through and bump the max anytime we find a superior candidate
        for (int j = 0; j < points.Count; j++)
        {
            if (max < Convert.ToSingle(points[j][columnName]))
            {
                max = Convert.ToSingle(points[j][columnName]);
            }
        }
        return max;
    }

    private float FindMinValue(string columnName)
    {



        return min;
    }

}
