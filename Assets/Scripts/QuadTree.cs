using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTree : MonoBehaviour
{
    Vector3[] datapoints;
    int treeheight;

    private void Start()
    {
        Debug.Log("QuadTree:Start: an instance of the Quad Tree was made");
        datapoints = GetComponent<DataProcesser>().data_points;
        treeheight = GetComponent<UserInputScript>().Resolution;
    }
    class Node
    {

    }

    class Tree
    {

    }
}
