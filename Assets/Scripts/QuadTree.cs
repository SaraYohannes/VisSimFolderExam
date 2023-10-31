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
    }
}
