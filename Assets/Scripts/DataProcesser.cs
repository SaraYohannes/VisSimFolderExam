using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataProcesser : MonoBehaviour
{
    public Vector3[] data_points;
    private void Awake()
    {
        string datapoint_txt = "Assets/Datafiles/newhoydedata.txt";

        if(File.Exists(datapoint_txt))
        {
            //We can read files
            Debug.Log("DataProcessor:Awake: file exists");
            ReadFile(datapoint_txt);
        }
    }

    void ReadFile(string datapoint_path)
    {
        StreamReader file = new StreamReader(datapoint_path);

        if(file != null )
        {
            // read file
            int line_counter = int.Parse(file.ReadLine());
            data_points = new Vector3[line_counter];
            int counter = 0;
            while (!file.EndOfStream)
            {
                string temp_point_line = file.ReadLine();
                string[] point_line = temp_point_line.Split(' ');
                
                Vector3 temp_v = new Vector3();

                temp_v.x = float.Parse(point_line[0]);
                temp_v.y = float.Parse(point_line[1]);
                temp_v.z = float.Parse(point_line[2]);

                data_points[counter] = temp_v;

                counter++;
            }
            Debug.Log("DataProcessor: ReadFile: if/else - while: the While statement is done and file-reading is done.");
            //create an instance of QuadTree()
            QuadTree quadTree = new QuadTree();
        }
        else
        {
            // there was a problem with StreamReader
            Debug.Log("DataProcessor:ReadFile: if/else - else: there is a problem with StreamReader");
        }
    }
}
