using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTree
{
    public class Node
    {
        // data to compare datapoints with
        float xmin, xmax, zmin, zmax;
        // place to store datapoints
        public List<Vector3> datapoint = new List<Vector3>();
        // is the list empty?
        public bool b_emptyList = true;
        // list threshold??
        public int threshold = 150;
        public int current_nr = 0;
        // node height, tree height
        public int nodeHeight;
        // children nodes
        public Node north, east, south, west;
        // the point representing the Node
        public Vector3 rep_point = new Vector3();
        // initialize everything which needs to be initialized
        public Node(float minx, float maxx, float minz, float maxz, int nodeheight)
        {
            xmax = minx; xmax = maxx; zmin = minz; zmax = maxz;

            // find representative point x and z RETHINK!!!!
            rep_point.x = minx + ((maxx - minx) / 2f);
            rep_point.z = minz + ((maxz - minz) / 2f);
            rep_point.y = 0f;

            nodeHeight = nodeheight;

            north = null;
            east = null;
            south = null;
            west = null;
        }

        /*public Node north, east, south, west;
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
        //    }
        //}*/
    }

    public class Tree
    {
        private QuadTree.Node rootNode;
        public Tree() 
        { 
            rootNode = null;        
        }
        public Tree (Vector3[] data_points, int treeheight, float minX, float maxX, float minZ, float maxZ)
        {
            rootNode = null;
            int nodeheight = 0;
            insertNodes(nodeheight, treeheight, minX, maxX, minZ, maxZ, rootNode);
            Debug.Log("QuadTree:Tree:Tree: Tree was just successfully finished!");

        }
        QuadTree.Node insertNodes(int nodeheight, int treeheight, float minX, float maxX, float minZ, float maxZ, QuadTree.Node root)
        {
            // make root node if it isn't made yet
            if (root == null)
            {
                Debug.Log("QuadTree:Tree:insertNodes: root node will be made.");
                root = new QuadTree.Node(minX, maxX, minZ, maxZ, nodeheight);
            }
            // this node is the hight of the tree, then stop
            if (treeheight == nodeheight)
            {
                Debug.Log("QuadTree:Tree:insertNodes: tree will be finished");
                return new QuadTree.Node(minX, maxX, minZ, maxZ, nodeheight);
            }

            // new x/z values
            float Xmid = (maxX - minX) / 2;
            float Zmid = (maxZ - minZ) / 2; 

            /// A quick reminder <3
            
            QuadTree.Node node = new QuadTree.Node(minX, maxX, minZ, maxZ, nodeheight);

            node.north = new QuadTree.Node(minX, Xmid, Zmid, maxZ, nodeheight + 1);
            node.east = new QuadTree.Node(Xmid, maxX, Zmid, maxZ, nodeheight + 1);
            node.south = new QuadTree.Node(Xmid, maxX, minZ, Zmid, nodeheight + 1);
            node.west = new QuadTree.Node(minX, Xmid, minZ, Zmid, nodeheight + 1);
    
            return node;
        }
        ~Tree() 
        {
            
        }
            /*
            // NORTH
            // X = XMIN -> XMID
            // Z = ZMID -> ZMAX

            // EAST
            // X = XMID -> XMAX
            // Z = ZMID -> ZMAX

            // SOUTH
            // X = XMID -> XMAX
            // Z = ZMIN -> ZMID

            // WEST
            // X = XMIN -> XMID
            // Z = ZMIN -> ZMID
            */
            /*int counter = 0;
            if (root == null)
            {
                root = new QuadTree.Node(minX, maxX, minZ, maxZ, counter);
            }
            if (treeheight == counter)
            {
                return new QuadTree.Node(minX, maxX, minZ, maxZ, counter);
            }
            float xmin, xmax, zmin, zmax, xmid, zmid;
            xmin = minX; xmax = maxX;
            zmin = minZ; zmax = maxZ;
            // for as long as the nodes children aren't yet the correct height, continue making nodes and inserting them
            while (root.nodeHeight<treeheight)
            {
                // initialize node with correct x/z values
                QuadTree.Node next = null;
                // find x and z values
                xmid = xRange(xmin, xmax);
                zmid = zRange(zmin, zmax);
                // new max values for next loop
                xmax = xmid; zmax = zmid; 
                counter++;
            }*/
        /*public Tree(Vector3[] data_points, int treeheight, float minX, float maxX, float minZ, float maxZ)
        {
            Debug.Log("QuadTree:Tree:Tree: A class was successfully constructed");
            rootNode = null;

            int nodeCounter = FinalNumberPerfectNodes(treeheight);
            int NodeHeight = 0;
            float minx, minz, maxx, maxz;
            minx = minX;
            minz = minZ;
            maxx = maxX;
            maxz = maxZ;
            while (treeheight > NodeHeight)
            {

                // what x/z ranges does the node have?
                // x/z values for each node
                float midX = xRange(minx, maxx);
                float midZ = zRange(minz, maxz);
                /*
                // 0<=x<0.5 ==> minX <= x < (maxX/2)
                    // 0<=y<0.5 ==> minZ <= z < (maxZ/2)
                    // 0.5<=y<1 ==> (maxZ/2) <= z < maxZ
                // 0.5<=x<1 ==> (maxX/2) <= x < maxX
                    // 0<=y<0.5 ==> minZ <= z < (maxZ/2)
                    // 0.5<=y<1 ==> (maxZ/2) <= z < maxZ
                
                // which height is it on?
                NodeHeight = NodeHeightCalculator(NodeHeight, treeheight, nodeCounter);
                InsertNode(NodeHeight, treeheight, minX, maxX, minZ, maxZ);
            }
        }
        float xRange(float min, float max)
        {
            return (max - min) / 2f;
        }
        float zRange(float min, float max)
        {
            return (max - min) / 2f;
        }
        int FinalNumberPerfectNodes(int treeheight) ///LOGIC SHOULD BE FINE
        {
            int nodeCounter = 1;
            if (treeheight == 0)
                return 1;
            else
            {
                for (int i = 1; i < treeheight+1; i++)
                {
                    nodeCounter = nodeCounter * 4;
                }
            }
            return nodeCounter;
        }
        int NodeHeightCalculator(int Nodeheight, int Treeheight, int nodeCounter)
        {
            
            return Nodeheight;
        }
        void InsertNode(int nodeheight, int treeheight, float minX, float maxX, float minZ, float maxZ)
        {
            // if rootNode does not exist- make it
                // increase NodeHeight
                // if we have 1/4/16/...
                // (previous number * 4, except if previous =0, then increase with 1


            // add root and their appropriate x/z values and nodeheight
        }
        */
    }
}
