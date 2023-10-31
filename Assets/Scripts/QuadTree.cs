using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class QuadTree
{
    public class Node
    {
        // data to compare datapoints with
        public float xmin, xmax, zmin, zmax;
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
        public QuadTree.Node[] childrenNodes = new QuadTree.Node[4];
        // the point representing the Node
        public Vector3 rep_point = new Vector3();
        // initialize everything which needs to be initialized
        public Node(float minx, float maxx, float minz, float maxz, int nodeheight)
        {
            this.xmin = minx; 
            this.xmax = maxx; 
            this.zmin = minz; 
            this.zmax = maxz;

            // find representative point x and z RETHINK!!!!
            rep_point.x = minx + ((maxx - minx) / 2f);
            rep_point.z = minz + ((maxz - minz) / 2f);
            rep_point.y = 0f;

            nodeHeight = nodeheight;

            //childrenNodes[0] = null;
            //childrenNodes[1] = null;
            //childrenNodes[2] = null;
            //childrenNodes[3] = null;
        }
        public void yHeight()
        {
            int ycounter = 0;
            float ycombined= 0f;
            foreach (var point in datapoint)
            {
                ycombined = ycombined + point.y;
                ycounter++;
            }

            if (ycounter > 0)
            {
                rep_point.y = (ycombined / ycounter);
            }
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
            int nodeheight = 0;
            rootNode = new QuadTree.Node(minX, maxX, minZ, maxZ, nodeheight);
            treeheight = 2;
            insertNodes(nodeheight, treeheight, minX, maxX, minZ, maxZ, rootNode);
            Debug.Log("QuadTree:Tree:Tree: Tree was just successfully finished!");
            // insert datapoints!
            Debug.Log("QuadTree.Tree.Tree: insertDatapoints is being called");
            insertDatapoints(data_points, treeheight, rootNode);
        }
        public QuadTree.Node traversal(int treeheight, QuadTree.Node currentNode)
        {
            if (currentNode.nodeHeight != treeheight)
            {
                foreach(QuadTree.Node child in currentNode.childrenNodes)
                {
                    traversal(treeheight, child);
                }
            }
            return currentNode;
        }
        // insert datapoints! (datapoints, rootNode)
        public void insertDatapoints(Vector3[] datapoints, int treeheight, QuadTree.Node currentNode)
        {
            Debug.Log("QuadTree:Tree:insertDatapoints: treeheight: " + treeheight);
            Debug.Log("QuadTree:Tree:insertDatapoints: currentNode height: " + currentNode.nodeHeight);
            Debug.Log("QuadTree:Tree:insertDatapoints: currentchild height: " + currentNode.childrenNodes[0].nodeHeight);
            Debug.Log("QuadTree:Tree:insertDatapoints: currentgrandchild height: " + currentNode.childrenNodes[0].childrenNodes[0].nodeHeight); 
            if (currentNode.nodeHeight == treeheight)
            {
                Debug.Log("QuadTree:Tree:insertDatapoints: current node is at the bottom of tree and point is being inserted in thedatalist");
                // insert information here?
                currentNode.datapoint.Add(datapoints[0]);
            }
            else
            {
                Debug.Log("QuadTree:Tree:insertDatapoints: we've entered foreach");
                foreach (var point in datapoints)
                {
                    Debug.Log("point: " + point);
                    /*Debug.Log("QuadTree:Tree:insertDatapoints: point.x = " + point.x);
                    Debug.Log("QuadTree:Tree:insertDatapoints: xmin = " + currentNode.xmin);
                    Debug.Log("QuadTree:Tree:insertDatapoints: xmax = " + currentNode.xmax);
                    Debug.Log("QuadTree:Tree:insertDatapoints: point.z = " + point.z);
                    Debug.Log("QuadTree:Tree:insertDatapoints: zmin = " + currentNode.zmin);
                    Debug.Log("QuadTree:Tree:insertDatapoints: zmax = " + currentNode.zmax);*/
                    if (currentNode.xmin <= point.x && currentNode.xmax >= point.x &&
                        currentNode.zmin <= point.z && currentNode.zmax >= point.z)
                    {
                        int quadrant = childQuadrant(point, currentNode);
                        insertDatapoints(new Vector3[] { point }, treeheight, currentNode.childrenNodes[quadrant]);
                    }
                }   
            }
        }
        // which quadrant??
        private int childQuadrant(Vector3 point, QuadTree.Node currentNode)
        {
            float Xmid = (currentNode.xmax - currentNode.xmin) / 2;
            float Zmid = (currentNode.zmax - currentNode.zmin) / 2;

            if (point.x < Xmid)
            {
                if (point.z < Zmid)
                {
                    return 3;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (point.z < Zmid)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }

        }
        public void findHeights (int treeheight, QuadTree.Node currentNode)
        {
            if(currentNode.nodeHeight == treeheight)
            {
                currentNode.yHeight();
            }
            else
            {
                foreach (QuadTree.Node child in currentNode.childrenNodes)
                {
                    findHeights(treeheight, child);
                }

            }
        }
        QuadTree.Node insertNodes(int nodeheight, int treeheight, float minX, float maxX, float minZ, float maxZ, QuadTree.Node root)
        {   
            // Make root node if it isn't made yet
            if (root == null)
            {
                Debug.Log("QuadTree:Tree:insertNodes: root node will be made.");
                root = new QuadTree.Node(minX, maxX, minZ, maxZ, nodeheight);
            }

            // If this node is the height of the tree, then stop
            if (treeheight == nodeheight)
            {
                Debug.Log("QuadTree:Tree:insertNodes: tree will be finished");
                return root;
            }

            // New x/z values
            float Xmid = (maxX - minX) / 2;
            float Zmid = (maxZ - minZ) / 2;

            // Check if child nodes are null and create if necessary
            if (root.childrenNodes[0] == null)
                root.childrenNodes[0] = new QuadTree.Node(minX, Xmid, Zmid, maxZ, nodeheight + 1);

            if (root.childrenNodes[1] == null)
                root.childrenNodes[1] = new QuadTree.Node(Xmid, maxX, Zmid, maxZ, nodeheight + 1);

            if (root.childrenNodes[2] == null)
                root.childrenNodes[2] = new QuadTree.Node(Xmid, maxX, minZ, Zmid, nodeheight + 1);

            if (root.childrenNodes[3] == null)
                root.childrenNodes[3] = new QuadTree.Node(minX, Xmid, minZ, Zmid, nodeheight + 1);

            // Recursively insert nodes for child nodes
            root.childrenNodes[0] = insertNodes(nodeheight + 1, treeheight, minX, Xmid, Zmid, maxZ, root.childrenNodes[0]);
            root.childrenNodes[1] = insertNodes(nodeheight + 1, treeheight, Xmid, maxX, Zmid, maxZ, root.childrenNodes[1]);
            root.childrenNodes[2] = insertNodes(nodeheight + 1, treeheight, Xmid, maxX, minZ, Zmid, root.childrenNodes[2]);
            root.childrenNodes[3] = insertNodes(nodeheight + 1, treeheight, minX, Xmid, minZ, Zmid, root.childrenNodes[3]);

            return root;
        }
        ~Tree() 
        {
            
        }
    }
}          