using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MeshMaker : MonoBehaviour
{
    Mesh mesh;
    MeshFilter filter;
    public List<Vector3> regular_triangulation;
    List<List<List<Vector3>>> datalist_prSquare = new List<List<List<Vector3>>>();
    public List<int> T_list;
    List<Vector3> dp_ready;
    int resolution = 200;
    float minX, minY, minZ, maxX, maxY, maxZ, internalX, internalZ, internalY;

    private void Awake()
    {
        mesh = new Mesh();
        filter = GetComponent<MeshFilter>();
        filter.mesh = mesh;
    }
    private void Start()
    {
        dp_ready = GetComponent<DataProcesser>().mesh_vert;

        if (dp_ready != null)
        {
            getBoarder();
            // makeRegularTriangulation();
            otherRegTriangulation();
            // PrintTest();
        }
        else
        {
            Debug.Log("MeshMaker: Start else-: dp_ready was not ready");
        }
    }
    void getBoarder()
    {
        minX = float.MaxValue;
        minY = float.MaxValue;
        minZ = float.MaxValue;

        maxX = float.MinValue;
        maxY = float.MinValue;
        maxZ = float.MinValue;

        foreach (var point in dp_ready)
        {
            if (point.x < minX)
                minX = point.x;
            if (point.x > maxX)
                maxX = point.x;

            if (point.y < minY)
                minY = point.y;
            if (point.y > maxY)
                maxY = point.y;

            if (point.z < minZ)
                minZ = point.z;
            if (point.z > maxZ)
                maxZ = point.z;
        }
        internalX = maxX - minX;
        Debug.Log("maxX - minX = internalX | " + maxX + " - " + minX + " = " + internalX); 
        internalY = maxY - minY;
        Debug.Log("maxY - minY = internalY | " + maxY + " - " + minY + " = " + internalY);
        internalZ = maxZ - minZ;
        Debug.Log("maxZ - minZ = internalZ | " + maxZ + " - " + minZ + " = " + internalZ);
    }
    void otherRegTriangulation ()
    {
        float xAxisStep = internalX / resolution;
        float zAxisStep = internalZ / resolution;
        Debug.Log("the Internal values of the plane should be: x-" + internalX + " z-" + internalZ);
        // int zCounter = 0;
        // int xCounter = 0;

        Debug.Log("maxX value is: " + maxX + " | maxZ value is: " + maxZ);
        Debug.Log("xAxisStep: " + xAxisStep + " | zAxisStep: " + zAxisStep);
        /*
        foreach (Vector3 p in dp_ready)
        {
            for (float row = minZ; row < maxZ; row = row + zAxisStep)
            {
                for(float col = minX;  col < maxX; col = col + xAxisStep)
                {
                    if (p.x > col &&  p.x < col + xAxisStep)
                    {
                        if (p.y > row && p.y < row + zAxisStep)
                        {
                            addingToArray(datalist_prSquare, p, xCounter, zCounter);
                        }
                    }
                    xCounter++;
                }
                zCounter++;
                xCounter = 0;
            }
            zCounter = 0;
        }
        */
    }
    void addingToArray(List<List<List<Vector3>>> datalist_prSquare, Vector3 p, int x, int z)
    {
        // if there aren't enough Lists in x directions, make more lists until we have reached x index
        while (datalist_prSquare.Count <= x)
        {
            datalist_prSquare.Add(new List<List<Vector3>> ());
        }
        // if there aren't enough Lists in z direction add more
        while (datalist_prSquare.Count <= z)
        {
            datalist_prSquare[x].Add(new List<Vector3> ());
        }
        // now there should be a valid list to add the Vector to!
        datalist_prSquare[x][z].Add(p);
    }
    void makeRegularTriangulation()
    {
        float xAxisStep = (200) / resolution;
        float zAxisStep = (200) / resolution;

        // Debug.Log("dp_ready.length is = " + dp_ready.Length);
        regular_triangulation = new List<Vector3>((resolution + 1) * (resolution + 1));
        int counter = 0;
        for (int i = 0; i < resolution + 1; i++)
        {
            for (int j = 0; j < resolution + 1; j++)
            {
                int index = i * (resolution + 1) + j;
                regular_triangulation.Add(new Vector3(j*xAxisStep, dp_ready[index].y, i*zAxisStep));
                
                counter++;
            }
        }

/*
        for (int i = 0; i < 20; i++)
        {
            Vector3 randomElement = GetRandomElement(regular_triangulation);
            Debug.Log("Random Element from Regular triangulation list nr " +i+" : " + randomElement);
        }
*/
        T_list = new List<int>();

        for (int row = 0; row < resolution; row++)
        {
            for (int column = 0; column < resolution; column++)
            {
                int point = (row * resolution) + row + column;

                T_list.Add(point);
                T_list.Add(point+(resolution)+1);
                T_list.Add(point+(resolution)+2);

                T_list.Add(point);
                T_list.Add(point+(resolution)+2);
                T_list.Add(point+1);
            }
        }
        AssignMesh(regular_triangulation, T_list);
    }

    //Vector3 GetRandomElement(List<Vector3> list)
    //{
    //    int randomIndex = Random.Range(0, list.Count);
    //    return list[randomIndex];
    //}

    void PrintTest ()
    {
        foreach (Vector3 p in datalist_prSquare[0][0])
        {
            Console.WriteLine(p);
        }
    }

    void AssignMesh(List<Vector3>vert, List<int>triangles)
    {
        mesh.Clear();
        mesh.vertices = vert.ToArray();
        mesh.triangles = triangles.ToArray();
        Debug.Log("Reached the end of AssignMesh");
    }
}
