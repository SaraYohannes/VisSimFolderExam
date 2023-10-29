using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTree
{
    public class Node
    { 
        public Node()
        {

        }
        ~Node() 
        { 
        
        }
    }

    public class Tree
    {
        public Tree() 
        { 
        
        }
        public Tree(Vector3[] data_points, int treeheight, float minX, float maxX, float minZ, float maxZ)
        {
            Debug.Log("QuadTree:Tree:Tree: A class was successfully constructed");
            /// make root
            /// uses treeheight to controll how many 'levels' of nodes to make
            /// 
        }
        ~Tree() 
        {
            
        }
        /// 
        /// 
        /// 
        /// 
        /// 

    }
}
