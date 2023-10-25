using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighbourhoods : MonoBehaviour
{
    private Vector3[] arr;
    private void Awake()
    {
        arr = new Vector3[25];
        BuildArray();
        DebugCheckerMineDontTouchIt();
        MakeNeighbours();
    }
    private void MakeNeighbours()
    {

    }
    private void DebugCheckerMineDontTouchIt()
    {
        for (int i = 0; i < arr.Length; i++)
        {
            Debug.Log("Vector " + i + ": " + arr[i]);
        }
    }
    void BuildArray()
    {
        int a = 0;
        for (int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                arr[a] = new Vector3(i, 0, j);
                a++;
            }
        }
    }
}
