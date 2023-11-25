using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
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
        public int b_emptyList;
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
        public Node() { }
        public Node(float minx, float maxx, float minz, float maxz, int nodeheight)
        {
            this.xmin = minx; 
            this.xmax = maxx; 
            this.zmin = minz; 
            this.zmax = maxz;

            rep_point.x = minx + ((maxx - minx) / 2f);
            rep_point.z = minz + ((maxz - minz) / 2f);
            rep_point.y = 0f;

            b_emptyList = 0;

            nodeHeight = nodeheight; //TODO: consider having height 7? 21'504 leaf nodes --> recall that the instatiated gameObjects are not the entire dataset. Most is not shown on screen

            childrenNodes[0] = null; childrenNodes[1] = null; childrenNodes[2] = null; childrenNodes[3] = null;
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
            else
            {
                Debug.Log("QuadTree: Node: yHeight: else: there are no datapoints in this node.");
            }
        }
    }

    public class Tree
    {
        GameObject cube = UnityEngine.GameObject.CreatePrimitive(PrimitiveType.Cube);
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
            // PrintQuadTree(rootNode);
            // insertDatapoints(data_points, treeheight, rootNode);
        }
        public QuadTree.Node traversal(Vector3 point, int treeheight, QuadTree.Node currentNode)
        {
            // Debug.Log("QuadTree:QuadTree.Node traversal: we are in the traversal function");
            if (currentNode == null)
                return null;
            
            if (currentNode.nodeHeight == treeheight) // pointdata fits here?
            {
                if (currentNode.childrenNodes[0] == null) // not necessary?
                {
                    return currentNode;
                }
            }
            else
            {
                if (currentNode.childrenNodes[0] != null) // not necessary?
                {
                    int correctChild = childQuadrant(point, currentNode);
                    traversal(point, treeheight, currentNode.childrenNodes[correctChild]);
                }
            }

            return currentNode;
        }
        public void insertDatapoints(Vector3[] datapoints, int treeheight, QuadTree.Node rootNode)
        {
            int counter = 0;
            // int counter2 = 0;
            int counter3 = 0;
            int counter4 = 0;
            List<Node> validNode = new List<Node>();
            foreach (var point in datapoints)
            {
                counter++;
                if (counter == 1001)
                {
                    break;
                }
                // make a copy of rootNode so we can manipulate every single node in QuadTree
                QuadTree.Node currentNode = new QuadTree.Node();
                currentNode = copyNode(rootNode, currentNode);
                // traverse tree to find correct node
                currentNode = traversal(point, treeheight, currentNode);
                if (currentNode == null)
                {
                    counter3++;
                    continue;
                }
                // insert data into node
                currentNode.datapoint.Add(point);
                // change information in node -- TODO: I might not need this? or b_emptyList at all
                //if (currentNode.b_emptyList == 0 && currentNode.datapoint.Count > 0)
                //{
                //    counter2++;
                //    currentNode.b_emptyList = 1;
                //}
                // check if node is in list, if not add
                if (currentNode.datapoint.Count > 0 && !validNode.Contains(currentNode))
                {
                    counter4++;
                    validNode.Add(currentNode);
                }
            }
            foreach (var node in validNode)
            {
                // get the height and rep point of this node
                node.yHeight();
                
                // find its triangle and neighbors
                    // insert into file index and neighbor info
                // keep track of how many triangles
                // add to the file at the very end, pre-append (i think? how many lines there are in file
            }
            foreach (var node in validNode)
            {
                Debug.Log("Number of data in node is: " + node.datapoint.Count);
                UnityEngine.Object.Instantiate(cube, node.rep_point, UnityEngine.Quaternion.identity);
            }
        }
        // which quadrant??
        private int childQuadrant(Vector3 point, QuadTree.Node currentNode)
        {
            float Xmid = (currentNode.xmax - currentNode.xmin) / 2;
            float Zmid = (currentNode.zmax - currentNode.zmin) / 2;

            if (point.x < Xmid)
            {
                currentNode.xmax = Xmid;
                if (point.z < Zmid)
                {
                    currentNode.zmax = Zmid;
                    return 3;
                }
                else
                {
                    currentNode.xmin = Xmid;
                    return 0;
                }
            }
            else
            {
                currentNode.zmin = Xmid;
                if (point.z < Zmid)
                {
                    currentNode.zmax = Zmid;
                    return 2;
                }
                else
                {
                    currentNode.xmin = Xmid;
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
                // Debug.Log("QuadTree:Tree:insertNodes: root node will be made.");
                root = new QuadTree.Node(minX, maxX, minZ, maxZ, nodeheight);
            }

            // If this node is the height of the tree, then stop
            if (treeheight == nodeheight)
            {
                // Debug.Log("QuadTree:Tree:insertNodes: tree will be finished");
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
        QuadTree.Node copyNode(QuadTree.Node rootNode, QuadTree.Node tempNode)
        {
            tempNode.xmin = rootNode.xmin; tempNode.xmax = rootNode.xmax; 
            tempNode.zmin = rootNode.zmin; tempNode.zmax = rootNode.zmax;

            tempNode.rep_point = new Vector3(rootNode.rep_point.x, rootNode.rep_point.y, rootNode.rep_point.z);

            tempNode.b_emptyList = rootNode.b_emptyList;
            
            tempNode.nodeHeight = rootNode.nodeHeight;

            tempNode.childrenNodes[0] = new QuadTree.Node();
            tempNode.childrenNodes[1] = new QuadTree.Node();
            tempNode.childrenNodes[2] = new QuadTree.Node();
            tempNode.childrenNodes[3] = new QuadTree.Node();

            tempNode.childrenNodes[0] = rootNode.childrenNodes[0];
            tempNode.childrenNodes[1] = rootNode.childrenNodes[1];
            tempNode.childrenNodes[2] = rootNode.childrenNodes[2];
            tempNode.childrenNodes[3] = rootNode.childrenNodes[3];

            tempNode.datapoint = new List<Vector3>(rootNode.datapoint);

            return tempNode;
        }

        public void PrintQuadTree(QuadTree.Node node, string prefix = "")
        {
            if (node == null)
            {
                Debug.Log("rootNode was null");
                return;
            }

            Debug.Log($"{prefix} Node: {node} Bounds: {node.xmin}, {node.xmax}, {node.zmin}, {node.zmax}");

            // Recursive call for each child node
            for (int i = 0; i < 4; i++)
            {
                PrintQuadTree(node.childrenNodes[i], prefix + "  ");
            }
        }

        ~Tree() 
        {
            
        }
    }
}          