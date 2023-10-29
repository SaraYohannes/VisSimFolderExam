using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTree
{
    public class Node
    {
        public Node north, east, south, west;
        public int treeHeight, nodeHeight;
        List<Vector3> datapoints = new List<Vector3>();
        public float minX, minZ, maxX, maxZ;
        Vector3 rep_point;
        bool b_emptyList;
        public Node(float minx, float maxx, float minz, float maxz, int nodeheight, int treeheight)
        {
            minX = minx;
            minZ = minz;
            maxX = maxx;
            maxZ = maxz;
            nodeHeight = nodeheight;
            treeHeight = treeheight;
            north = null;
            east = null;
            south = null;
            west = null;
        }
        //~Node() 
        //{ 
        
        //}
        //void NodeValueRange()
        //{

        //}
        //void NodeRepValue()
        //{
        //    /// find average of all Z values in datapoints
        //}
        //void MoreHeight(Vector3[] nodeChildren, int treeHeight, int nodeHeight)
        //{
        //    ///travel inside each child and make another set of children unless nodeHeight=> than treeheight
        //    /*if (nodeHeight < treeHeight)
        //    {
        //        for (int i = 0; i < nodeChildren.Length; i++)
        //        {
        //            nodeChildren[i] = CreateNode();// nodeChildren[i] = new QuadTree.Node(2f, 2f, minX, maxX, minZ, maxZ, nodeHeight, treeHeight);

        //        }
        //    }*/
        //}
        
    }

    public class Tree
    {
        private QuadTree.Node rootNode;
        public Tree() 
        { 
        
        }
        public Tree(Vector3[] data_points, int treeheight, float minX, float maxX, float minZ, float maxZ)
        {
            int NodeHeight = 0;
            Debug.Log("QuadTree:Tree:Tree: A class was successfully constructed");
            while (treeheight>NodeHeight)
            {
                // insert nodes in tree
            }
        }
        void InsertNode()
        {
            // if rootNode does not exist- make it
                // increase NodeHeight
                // if we have 1/4/16/...
                // (previous number * 4, except if previous =0, then increase with 1


            // add root and their appropriate x/z values and nodeheight
        }
        ~Tree() 
        {
            
        }
    }
}
