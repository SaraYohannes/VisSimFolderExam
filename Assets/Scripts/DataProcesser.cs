using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.IO;
using UnityEngine;
using JetBrains.Annotations;
using System;

public class DataProcesser : MonoBehaviour
{
    public System.Numerics.Vector3[] data_points;
    public System.Numerics.Vector3[] dp_processed;
    public UnityEngine.Vector3[] dp_ready;
    [SerializeField] public GameObject pointPrefab;
    int line_counter;
    float minX, minY, minZ, maxX, maxY, maxZ;

    private void Start()
    {
        string datapoint_txt = "Assets/Datafiles/newhoydedata.txt";
        
        if(File.Exists(datapoint_txt)) 
        {
            //We can read files
            ReadFile(datapoint_txt); 
        }
        else
        {
            Debug.Log("DataProcessor:Start: file does not exist");
        }
    }
    void ReadFile(string datapoint_path)
    {
        StreamReader file = new StreamReader(datapoint_path);

        if(file != null )
        {
            // read file
            line_counter = int.Parse(file.ReadLine());
            data_points = new System.Numerics.Vector3[line_counter];
            int counter = 0;
            while (!file.EndOfStream)
            {
                string temp_point_line = file.ReadLine();
                string[] point_line = temp_point_line.Split(' ');

                System.Numerics.Vector3 temp_v = new System.Numerics.Vector3();

                temp_v.X = float.Parse(point_line[0]);
                temp_v.Z = float.Parse(point_line[1]);
                temp_v.Y = float.Parse(point_line[2]);

                data_points[counter] = temp_v;

                counter++;
            }
            TranslatePoints();
            showPoints();
        }
        else
        {
            // there was a problem with StreamReader
            Debug.Log("DataProcessor:ReadFile: if/else - else: there is a problem with StreamReader");
        }
    }
    void showPoints() // THIS HAS BEEN CHANGED TO FIT ONE OF THE TASKS FOR THE EXAM
    {
        float scale = 0.8f;
        int counter = 0;
        for (int i = 0;  i < dp_ready.Length; i++)
        {
            counter++;
            if (counter % 1000 == 0)
            {
                UnityEngine.Vector3 scaledpos = dp_ready[i] * scale;
                Instantiate(pointPrefab, scaledpos, UnityEngine.Quaternion.identity);                
            }
        }
    }
    void TranslatePoints()
    {
        minX = float.MaxValue;
        minY = float.MaxValue;
        minZ = float.MaxValue;

        maxX = float.MinValue;
        maxY = float.MinValue;
        maxZ = float.MinValue;

        foreach(var point in data_points)
        {
            if (point.X < minX)
                minX = point.X;
            if (point.X > maxX)
                maxX = point.X;

            if (point.Y < minY)
                minY = point.Y;
            if (point.Y > maxY)
                maxY = point.Y;

            if (point.Z < minZ)
                minZ = point.Z;
            if (point.Z > maxZ)
                maxZ = point.Z;
        }

        float newX = -minX;
        float newY = -minY;
        float newZ = -minZ;

        System.Numerics.Matrix4x4 translationMatrix = System.Numerics.Matrix4x4.CreateTranslation(newX, newY, newZ);

        dp_processed = new System.Numerics.Vector3[line_counter];
        int counter = 0;
        foreach(var point in data_points)
        {
            System.Numerics.Vector3 temp = System.Numerics.Vector3.Transform(point, translationMatrix);
            dp_processed[counter] = temp;
            counter++;
        }
        Converter();
    }
    void Converter()
    {
        dp_ready = new UnityEngine.Vector3[line_counter];
        int counter = 0;
        foreach(var point in dp_processed)
        {
            dp_ready[counter].x = point.X;
            dp_ready[counter].y = point.Y;
            dp_ready[counter].z = point.Z;
            counter++;
        }
    }
}
