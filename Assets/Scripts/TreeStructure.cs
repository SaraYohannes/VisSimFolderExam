using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeStructure : MonoBehaviour
{
    /// quad structure
    [SerializeField] public TreeNode Node;
    // get radius from pointCloudProcesser
    [SerializeField] public Vector3 boundLength;
    private TreeNode Root;
    // start
    private void Start()
    {
        Root = GetComponent<TreeNode>(); // root of tree
        // set up node        
        BuildTree();
    }
    // update tree?    
    private void Update()
    {
        
    }
    // build tree (algorithm)
    void BuildTree()
    {
        // check if tree is done
    }

    // functionality
    void insertNode()
    {
        // insert
    }
    void removeNode()
    {
        // remove
    }
    void searchNode()
    {
        // search

    }

}
