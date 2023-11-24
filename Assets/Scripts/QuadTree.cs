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
        private QuadTree.Node rootNode;
        public int nodeCounterBecauseIneedThis = 0;
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
            // Debug.Log("QuadTree:Tree:Tree: Tree was just successfully finished!");
            Debug.Log("QuadTree:Tree:Tree: We have " + nodeCounterBecauseIneedThis + " nodes in the tree");
            // insert datapoints!
            Debug.Log("QuadTree.Tree.Tree: insertDatapoints is being called");
            insertDatapoints(data_points, treeheight, rootNode);
        }
        public QuadTree.Node traversal(Vector3 point, int treeheight, QuadTree.Node currentNode)
        {
            // Debug.Log(11111);
            if (currentNode == null)
                return null;
            
            if (currentNode.nodeHeight == treeheight) // pointdata fits here?
            {
                if (currentNode.childrenNodes[0] == null) // not necessary?
                {
                    return currentNode;
                }
                Debug.Log(5);
            }
            else
            {
                if (currentNode.childrenNodes[0] != null) // not necessary?
                {
                    int correctChild = childQuadrant(point, currentNode); //TODO: we don't have the pointdata. Need it to figure out which child to go to DONE?
                    traversal(point, treeheight, currentNode.childrenNodes[correctChild]);
                }
            }

            return currentNode;
        }
        // insert datapoints! (datapoints, rootNode)
        public void insertDatapoints(Vector3[] datapoints, int treeheight, QuadTree.Node currentNode)
        {
            int counter = 0;
            int counter2 = 0;
            int counter3 = 0;
            int counter4 = 0;
            List<Node> validNode = new List<Node>();
            Debug.Log("QuadTree.Tree.Tree: insertDatapoints: List validNode was made");
            foreach (var point in datapoints)
            {
                counter++;
                if (counter == 101)
                {
                    break;
                }
                // traverse tree to find correct node
                rootNode = traversal(point, treeheight, currentNode);
                if (rootNode == null)
                {
                    counter3++;
                    continue; //s-s-skippp!
                }
                // insert data into node
                rootNode.datapoint.Add(point);
                // change information in node
                Debug.Log("Is it true or not? " + rootNode.b_emptyList);
                if (rootNode.b_emptyList)
                {
                    counter2++;
                    rootNode.b_emptyList = false;
                }
                // check if node is in list, if not add
                if (!rootNode.b_emptyList && validNode.Contains(rootNode))
                {
                    counter4++;
                    validNode.Add(rootNode);
                }
            }
            Debug.Log("QuadTree.Tree.Tree: insertDatapoints: foreach point in datapoints is done " + counter);
            Debug.Log("QuadTree.Tree.Tree: insertDatapoints: there was a rootNode this many times " + counter3);
            Debug.Log("QuadTree.Tree.Tree: insertDatapoints: we had this many nodes change from true to false " + counter2);
            Debug.Log("QuadTree.Tree.Tree: insertDatapoints: we had this many valid nodes in the validNode list " + counter4);

            foreach (var node in validNode)
            {
                Debug.Log("QuadTree.Tree.Tree: insertDatapoints: foreach validNode find height");
                // get the height and rep point of this node
                node.yHeight();
                
                // find its triangle and neighbors
                    // insert into file index and neighbor info
                // keep track of how many triangles
                // add to the file at the very end, pre-append (i think? how many lines there are in file
            }
            Debug.Log("QuadTree.Tree.Tree: insertDatapoints: foreach node in ValidNode has a height");
            foreach (var node in validNode)
            {
                Debug.Log("RepPoint in node has height: " + node.rep_point);
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
            nodeCounterBecauseIneedThis++;
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
        ~Tree() 
        {
            
        }
    }
}          